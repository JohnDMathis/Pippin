using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel.DomainServices.Client;
using System.Linq;
using System.Collections.Generic;
using Pippin.Testing;
using MI.Model;
namespace MI.Authentication.Specs
{
    public class MockAuthenticationClient:LocalDomainClient
    {
        public bool AuthenticationKeyProvided { get; set; }
        public string userNameProvided { get; set; }
        public string passwordProvided { get; set; }
        public AppUser LoginTestUser;

        public MockAuthenticationClient(List<Entity> entities)
        {
            MockEntities = entities;
            QueriedEntities = new List<Entity>();
            Queries = new List<string>();
            InvokeOperations = new List<string>();
        }

        protected override IQueryable<Entity> Query(string queryName, IDictionary<string, object> parameters)
        {
            List<Entity> queryResult = null;
            int matchId = 0;
            Queries.Add(queryName);
            switch (queryName)
            {
                case "Login":
                    AuthenticationKeyProvided = (parameters["clientKey"] != null);
                    userNameProvided = (string)parameters["userName"];
                    passwordProvided = (string)parameters["password"];
                    if (userNameProvided == "FredT" && passwordProvided == "test")
                        queryResult = (from e in MockEntities select e).ToList();
                    else
                        queryResult = new List<Entity>();
                    break;

                //case "GetServiceEventsForMonth":
                //    queryResult = MockEntities.Where(e => e.GetType() == typeof(ScheduledEvent)).ToList();
                //    break;
            }
                return queryResult.AsQueryable<Entity>();
        }

        protected override IEnumerable<ChangeSetEntry> Submit(EntityChangeSet changeSet)
        {
            return null;
        }

        protected override object Invoke(string operationName, IDictionary<string, object> parameters)
        {
            InvokeOperations.Add(operationName);
            object result = null;
            switch (operationName)
            {
                case "CheckLogin":
                    result = EntryScreenSpec.CheckLoginTestResult;
                    break;
                    
                case "GetAuthenticatedUser":
                    result = LoginTestUser;
                    break;

                case "InvokeLogin":
                    AuthenticationKeyProvided = (parameters["clientKey"] != null);
                    userNameProvided = (string)parameters["userName"];
                    passwordProvided = (string)parameters["password"];
                    if (userNameProvided == "FredT" && passwordProvided == "test")
                        result = LoginTestUser;
                    else
                        result = null;
                    break;

            }
            return result;
        }

    }
}
