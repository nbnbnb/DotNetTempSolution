using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Entities
{
    public class Batch : IEquatable<Batch>
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private short _bussinessId;
        private int _providerId;
        private DateTime _checkDate;
        public long BatchId { get; set; }

        public Batch(DateTime checkDate, DateTime startDate, DateTime endDate, short bussinessId, int providerId)
        {
            this._startDate = startDate;
            this._endDate = endDate;
            this._bussinessId = bussinessId;
            this._providerId = providerId;
            this._checkDate = checkDate;
        }
        public bool Equals(Batch other)
        {
            return other._checkDate >= this._startDate &&
                other._checkDate < this._endDate &&
                other._bussinessId == this._bussinessId &&
                other._providerId == this._providerId;
        }
    }
}
