export class GeneratorService {

  public static serviceName = 'wfGeneratorService';

  urlBase = Umbraco.Sys.ServerVariables.UmbracoUrls.workflowDataGeneratorApiBaseUrl;

  constructor(
    private $http,
    private umbRequestHelper) {
  }

  request = (method: string, url: string, data?: object) =>
    this.umbRequestHelper.resourcePromise(method === 'POST' ? this.$http.post(url, data) : this.$http.get(url), 'Something broke');

  generate = (queryModel) => this.request('POST', this.urlBase, queryModel);

  reset = () => this.request('POST', `${this.urlBase}/reset`);

  status = () => this.request('GET', `${this.urlBase}/status`);

  getTour = () => this.request('GET', `${this.urlBase}/get-tour`);
}
