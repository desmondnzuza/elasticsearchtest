using Elasticsearch.Net;
using ESGroupUpdator.Models;
using Nest;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ESGroupUpdator
{
    public class Indexer
    {
        internal void Index(ElasticSearchIndex index)
        {

            var elasticClient = GetClient();

            
                var observableBulkAll = elasticClient.BulkAll(index.Documents, b => b
                    .Index(index.Name)
                    .Type("entry")
                    .BackOffRetries(5)
                    .BackOffTime("30s")
                    .RefreshOnCompleted(true)
                    .MaxDegreeOfParallelism(4)
                    .Size(30000)
                );

                var bulkAllObserver = new BulkAllObserver(
                        onError: (e) => { throw e; },
                        onCompleted: () => DoSomething(index.Name),
                        onNext: (b) => DoSomethingElse((BulkAllResponse)b, index.Name)
                );

                observableBulkAll.Subscribe(bulkAllObserver);
        }

        private ElasticClient GetClient()
        {
            ConnectionSettings connectionSettings;
            StaticConnectionPool connectionPool;
            
            var nodes = new Uri[]
            {
                new Uri(ConfigurationManager.AppSettings["Elasticsearch"].ToString())
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);

            return new ElasticClient(connectionSettings);
        }

        private void DoSomethingElse(BulkAllResponse response, string name)
        {
            Console.WriteLine($"{name}\t: Moving to next item. Done with {response.Retries} retries on Page {response.Page} and status of {response.IsValid}");
        }

        private void DoSomething(string name)
        {
            Console.WriteLine($"{name}\t: Bulk Indexing complete");
        }
    }
}
