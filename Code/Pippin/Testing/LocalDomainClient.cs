using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.ServiceModel.DomainServices.Client;

namespace Pippin.Testing
{
    public abstract class LocalDomainClient : DomainClient
    {
        private SynchronizationContext _syncContext;

        private List<string> _queriesInProgress { get; set; }
        protected LocalDomainClient()
        {
            _syncContext = SynchronizationContext.Current;
            _queriesInProgress = new List<string>();
        }

        #region [ Query ]

        protected abstract IQueryable<Entity> Query(string queryName, IDictionary<string, object> parameters);

        //protected override sealed IAsyncResult BeginQueryCore(EntityQuery query, AsyncCallback callback, object userState)
        //{
        //    IQueryable<Entity> localQuery = Query(query);
        //    LocalAsyncResult asyncResult = new QueryAsyncResult(callback, userState, localQuery);
        //    this.synchronizationContext.Post(o => ((LocalAsyncResult)o).Complete(), asyncResult);
        //    return asyncResult;
        //} 


        protected override IAsyncResult BeginQueryCore(EntityQuery query, AsyncCallback callback, object userState)
        {
            Debug.WriteLine("BeginQueryCore(" + query.QueryName + ", " + _queriesInProgress.Count + ")");
            if (_queriesInProgress.Count != 0)
                Debug.WriteLine("...open queries found");
            _queriesInProgress.Add(query.QueryName);
            if (query.QueryName == "GetPermissionsForUser")
                Debug.WriteLine(query.QueryName);
            IQueryable<Entity> localQuery = Query(query.QueryName, query.Parameters);

            LocalAsyncResult asyncResult = new LocalAsyncResult(query.QueryName, callback, userState, localQuery);
            _syncContext.Post(delegate(object o)
                                  {
                                      ((LocalAsyncResult)o).Complete();
                                  }, asyncResult);
            return asyncResult;

        }

        protected override sealed QueryCompletedResult EndQueryCore(IAsyncResult asyncResult)
        {
            LocalAsyncResult localAsyncResult = (LocalAsyncResult)asyncResult;
            _queriesInProgress.Remove(localAsyncResult.QueryName);
            int c = (localAsyncResult.Entities == null) ? 0 : localAsyncResult.Entities.Count();

            var validationResults = new List<ValidationResult>();
            var includedEntities = new List<Entity>();
            return new QueryCompletedResult(localAsyncResult.Entities, includedEntities, c, validationResults);
        }

        #endregion [ Query ]

        #region [ Submit ]

        protected abstract IEnumerable<ChangeSetEntry> Submit(EntityChangeSet changeSet);

        protected override sealed IAsyncResult BeginSubmitCore(EntityChangeSet changeSet, AsyncCallback callback, object userState)
        {
            IEnumerable<ChangeSetEntry> operations = Submit(changeSet);

            LocalAsyncResult asyncResult = new LocalAsyncResult(callback, userState, changeSet, operations);
            _syncContext.Post(delegate(object o)
                                  {
                                      ((LocalAsyncResult)o).Complete();
                                  }, asyncResult);

            return asyncResult;
        }

        protected override sealed SubmitCompletedResult EndSubmitCore(IAsyncResult asyncResult)
        {
            LocalAsyncResult localAsyncResult = (LocalAsyncResult)asyncResult;
            return new SubmitCompletedResult(localAsyncResult.ChangeSet, localAsyncResult.Operations);
        }

        #endregion [ Submit ]

        #region [ Invoke ]

        protected abstract object Invoke(string operationName, IDictionary<string, object> parameters);

        protected override IAsyncResult BeginInvokeCore(InvokeArgs invokeArgs, AsyncCallback callback, object userState)
        {
            var returnData = Invoke(invokeArgs.OperationName, invokeArgs.Parameters);
            var asyncResult = new LocalAsyncResult(callback, userState, returnData);
            _syncContext.Post(o => ((LocalAsyncResult)o).Complete(), asyncResult);
            return asyncResult;
        }

        protected override sealed InvokeCompletedResult EndInvokeCore(IAsyncResult asyncResult)
        {
            var localAsyncResult = (LocalAsyncResult)asyncResult;
            return new InvokeCompletedResult(localAsyncResult.ReturnData);
        }

        #endregion [ Invoke ]

        #region [ Mock Items ]

        public virtual List<Entity> MockEntities { get; set; }
        public virtual List<Entity> QueriedEntities { get; set; }
        public virtual List<string> Queries { get; set; }
        public virtual List<string> InvokeOperations { get; set; }
        public virtual List<EntityChangeSet> SubmittedChanges { get; set; }

        #endregion [ Mock Items ]

        private sealed class LocalAsyncResult : IAsyncResult
        {
            private string _queryName;
            private AsyncCallback _callback;
            private object _asyncState;
            private bool _completed;
            private IEnumerable<Entity> _entities;
            private EntityChangeSet _changeSet;
            private IEnumerable<ChangeSetEntry> _operations;
            private object _returnData;

            public LocalAsyncResult(string queryName, AsyncCallback callback, object asyncState, IEnumerable<Entity> entities)
            {
                _queryName = queryName;
                _callback = callback;
                _asyncState = asyncState;
                _entities = entities;
            }

            public LocalAsyncResult(AsyncCallback callback, object asyncState, EntityChangeSet changeSet, IEnumerable<ChangeSetEntry> operations)
            {
                _callback = callback;
                _asyncState = asyncState;
                _changeSet = changeSet;
                _operations = operations;
            }
            public LocalAsyncResult(AsyncCallback callback, object asyncState, object returnData)
            {
                _callback = callback;
                _asyncState = asyncState;
                _returnData = returnData;
            }


            public string QueryName
            {
                get
                {
                    return _queryName;
                }
            }

            public EntityChangeSet ChangeSet
            {
                get
                {
                    return _changeSet;
                }
            }

            public IEnumerable<Entity> Entities
            {
                get
                {
                    return _entities;
                }
            }

            public IEnumerable<ChangeSetEntry> Operations
            {
                get
                {
                    return _operations;
                }
            }

            public object ReturnData
            {
                get
                {
                    return _returnData;
                }
            }


            public void Complete()
            {
                _completed = true;
                _callback(this);
            }

            #region IAsyncResult Members
            public object AsyncState
            {
                get
                {
                    return _asyncState;
                }
            }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    return null;
                }
            }

            public bool CompletedSynchronously
            {
                get
                {
                    return false;
                }
            }

            public bool IsCompleted
            {
                get
                {
                    return _completed;
                }
            }
            #endregion
        }
    }
}