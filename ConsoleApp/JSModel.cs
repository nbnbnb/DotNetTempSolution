using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5App.Models
{
    public class JSModel
    {
        public long SceneId { get; set; }
        public string SceneName { get; set; }
        public List<BasicItem> DestinationItems { get; set; }
        public List<BasicItem> StargingItems { get; set; }
        public List<TemplateItem> TemplateItems { get; set; }
        public string Remark { get; set; }
    }

    public class IdText
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class BasicItem
    {
        public List<IdText> ContinentInfos { get; set; }

        public List<IdText> CountryInfos { get; set; }

        public List<IdText> CityInfos { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }

    public class TemplateItem
    {
        public int TemplateId { get; set; }

        public int Percent { get; set; }
    }
}