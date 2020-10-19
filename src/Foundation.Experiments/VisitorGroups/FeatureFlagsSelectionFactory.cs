using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Rest;

namespace Foundation.Experiments.VisitorGroups
{
    public class FeatureFlagsSelectionFactory : ISelectionFactory
    {
        private static readonly Lazy<IExperimentationClient> ExperimentationClient = new Lazy<IExperimentationClient>(() => ServiceLocator.Current.GetInstance<IExperimentationClient>());

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType) => GetFeaturesList();
        
        private List<SelectListItem> GetFeaturesList()
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();
            var features = ExperimentationClient.Value.GetFeatureList();
            if (features == null)
                return selectItems;

            foreach (var flag in features)
                selectItems.Add(new SelectListItem { Text = flag.Name, Value = flag.Key });

            return selectItems.OrderBy(x => x.Text).ToList();
        }
    }
}