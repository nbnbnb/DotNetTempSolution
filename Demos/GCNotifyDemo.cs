using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos
{
    class GCNotifyDemo
    {
        // Variable for continual checking in the 
        // While loop in the WaitForFullGCProc method.
        static volatile bool checkForNotify = false;

        // Variable for suspending work 
        // (such servicing allocated server requests)
        // after a notification is received and then 
        // resuming allocation after inducing a garbage collection.
        static volatile bool bAllocate = false;

        // Variable for ending the example.
        static volatile bool finalExit = false;

        // Collection for objects that  
        // simulate the server request workload.
        static List<byte[]> load = new List<byte[]>();

        public static void Run()
        {
            try
            {
                // Register for a notification. 
                // maxGenerationThreshold 和 largeObjectHeapThreshold 说明
                // https://stackoverflow.com/questions/12403931/how-can-i-choose-parameters-for-gc-registerforfullgcnotification
                /*
                The larger the threshold value, the more allocations will occur between the notification and the full garbage collection.

                A larger threshold value provides more opportunities for the runtime to check for an approaching collection. 
                This increases the likelihood that you will be notified. However, you should not set the threshold too high because that results in a more allocations before the runtime induces the next collection.

                When you induce a collection yourself upon notification using a high threshold value, fewer objects are reclaimed than would be reclaimed by the runtime's next collection.

                The smaller the threshold value, the fewer the allocations between notification and the full garbage collection.
                */
                GC.RegisterForFullGCNotification(10, 10);
                Console.WriteLine("Registered for GC notification.");

                checkForNotify = true;
                bAllocate = true;

                // Start a thread using WaitForFullGCProc.
                Thread thWaitForFullGC = new Thread(new ThreadStart(WaitForFullGCProc));
                thWaitForFullGC.Start();

                // While the thread is checking for notifications in
                // WaitForFullGCProc, create objects to simulate a server workload.
                try
                {

                    int lastCollCount = 0;
                    int newCollCount = 0;

                    while (true)
                    {
                        if (bAllocate)
                        {
                            load.Add(new byte[1000]);
                            // 返回已经对对象的指定代进行的垃圾回收次数
                            newCollCount = GC.CollectionCount(2);
                            if (newCollCount != lastCollCount)
                            {
                                // Show collection count when it increases:
                                Console.WriteLine("Gen 2 collection count: {0} - lastCollCount：{1}", newCollCount, lastCollCount);
                                lastCollCount = newCollCount;
                            }

                            // For ending the example (arbitrary).
                            // 达到 500 次 GC 的时候，停止 Demo
                            if (newCollCount == 500)
                            {
                                finalExit = true;
                                checkForNotify = false;
                                break;
                            }
                        }
                    }
                }
                // OOM 之前，系统将执行 GC
                // 所以会收到 WaitForFullGCApproach 的通知
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("Out of memory.");
                }
                // OOM 后，关闭内存分配
                // 取消注册通知
                finalExit = true;
                checkForNotify = false;
                // 触发 WaitForFullGCApproach - Canceled
                GC.CancelFullGCNotification();

            }
            catch (InvalidOperationException invalidOp)
            {
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n" + invalidOp.Message);
            }
        }

        public static void OnFullGCApproachNotify()
        {

            Console.WriteLine("Redirecting requests.");

            // Method that tells the request queuing  
            // server to not direct requests to this server. 
            // 重定向请求
            RedirectRequests();

            // Method that provides time to 
            // finish processing pending requests. 
            // 处理正在进行中的请求
            FinishExistingRequests();

            // This is a good time to induce a GC collection
            // because the runtime will induce a full GC soon.
            // To be very careful, you can check precede with a
            // check of the GC.GCCollectionCount to make sure
            // a full GC did not already occur since last notified.
            // 现在是进行 GC 收集的好时机
            // 因为运行时将很快引发完整的 GC
            // 收集完成后，将会触发 WaitForFullGCComplete 
            GC.Collect();
            //GC.Collect(2, GCCollectionMode.Forced, true);
            Console.WriteLine("Induced a collection.");

        }


        public static void OnFullGCCompleteEndNotify()
        {
            // Method that informs the request queuing server
            // that this server is ready to accept requests again.
            // 允许请求
            AcceptRequests();
            Console.WriteLine("Accepting requests again.");
        }

        public static void WaitForFullGCProc()
        {
            while (true)
            {
                // CheckForNotify is set to true and false in Main.
                while (checkForNotify)
                {
                    // Check for a notification of an approaching collection.
                    GCNotificationStatus s = GC.WaitForFullGCApproach();
                    // 是否即将引发完整、阻碍性垃圾回收
                    if (s == GCNotificationStatus.Succeeded)
                    {
                        Console.WriteLine("WaitForFullGCApproach - GC Notification raised.");
                        // 要开始执行 GC(2) 了
                        // 通知系统重定向请求
                        // 关闭内存分配
                        OnFullGCApproachNotify();
                    }
                    else if (s == GCNotificationStatus.Canceled)
                    {
                        Console.WriteLine("WaitForFullGCApproach - GC Notification cancelled.");
                        break;
                    }
                    else
                    {
                        // This can occur if a timeout period
                        // is specified for WaitForFullGCApproach(Timeout) 
                        // or WaitForFullGCComplete(Timeout)  
                        // and the time out period has elapsed. 
                        Console.WriteLine("WaitForFullGCApproach - GC Notification not applicable.");
                        break;
                    }

                    // Check for a notification of a completed collection.
                    GCNotificationStatus status = GC.WaitForFullGCComplete();
                    // 完整、阻碍性垃圾回收是否已完成
                    if (status == GCNotificationStatus.Succeeded)
                    {
                        Console.WriteLine("WaitForFullGCComplete - GC Notification raised.");
                        // 阻塞回收已经完成
                        // 通知系统可以继续接收请求
                        OnFullGCCompleteEndNotify();
                    }
                    else if (status == GCNotificationStatus.Canceled)
                    {
                        Console.WriteLine("WaitForFullGCComplete - GC Notification cancelled.");
                        break;
                    }
                    else
                    {
                        // Could be a time out.
                        Console.WriteLine("WaitForFullGCComplete - GC Notification not applicable.");
                        break;
                    }
                }

                // 每 500ms 检测一次
                Thread.Sleep(500);
                // FinalExit is set to true right before  
                // the main thread cancelled notification.
                // 在主线程推出之前，finalExit 会设置为 true
                if (finalExit)
                {
                    break;
                }
            }

        }

        private static void RedirectRequests()
        {
            // Code that sends requests
            // to other servers.

            // Suspend work.
            bAllocate = false;

        }

        private static void FinishExistingRequests()
        {
            // Code that waits a period of time
            // for pending requests to finish.

            // Clear the simulated workload.
            load.Clear();

        }

        private static void AcceptRequests()
        {
            // Code that resumes processing
            // requests on this server.

            // Resume work.
            bAllocate = true;

        }
    }

}
