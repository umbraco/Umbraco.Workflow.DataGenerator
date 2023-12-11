type ObjectArray = Array<Record<string, any>>;

export class GeneratorDashboardController {
  public static controllerName = "Workflow.GeneratorDashboard.Controller";

  hasExistingUmbracoData = false;
  hasInstalledUmbracoData = false;
  hasInstalledWorkflowData = false;
  hasDismissedTour = false;
  workflowDataModel?: { groupCount: number, userCount: number, usersPerGroup: number, groupsPerWorkflow: number };

  generateState = 'init';
  resetState = 'init';

  baseProperties: ObjectArray = [
    {
      alias: "groupCount",
      view: "integer",
      label: "Number of approval groups",
      value: 2,
      min: 2,
    },
    {
      alias: "userCount",
      view: "integer",
      label: "Number of users",
      value: 2,
      min: 2,
    },
    {
      alias: "groupsPerWorkflow",
      view: "integer",
      label: "Number of groups per workflow",
      description: "Set to 0 for random",
      value: 0,
      min: 0,
    },
    {
      alias: "usersPerGroup",
      view: "integer",
      label: "Number of users per approval group",
      description: "Set to 0 for random",
      value: 0,
      min: 0,
    },
  ];

  tourComplete?: Function;
  tourEnd?: Function;

  constructor(private wfGeneratorService, private tourService, private eventsService) { }

  async $onInit() {
    const {
      hasExistingUmbracoData,
      hasInstalledUmbracoData,
      hasInstalledWorkflowData,
      hasDismissedTour,
      workflowDataModel,
    } = await this.wfGeneratorService.status();

    this.hasExistingUmbracoData = hasExistingUmbracoData;
    this.hasInstalledUmbracoData = hasInstalledUmbracoData;
    this.hasInstalledWorkflowData = hasInstalledWorkflowData;
    this.hasDismissedTour = hasDismissedTour;
    this.workflowDataModel = workflowDataModel;

    this.tour();

    // after installing the site, the application restarts and the view reloads
    // the status object will return the original request info, which needs to be re-submitted
    // to run the workflow configuration steps
    if (this.hasInstalledUmbracoData && !this.hasInstalledWorkflowData) {
      this.baseProperties.forEach(prop => {
        prop.value = this.workflowDataModel?.[prop.alias] ?? prop.value;
      });

      this.generate();
    }

    this.tourComplete = this.eventsService.on('appState.tour.complete', (_, tour) => {
      console.log(tour);
    });

    this.tourEnd = this.eventsService.on('appState.tour.end', (_, tour) => {
      console.log(tour);
    });
  }

  $onDestroy() {
    this.tourComplete?.();
    this.tourEnd?.();
  }

  async generate() {
    this.generateState = 'busy';
    await this.wfGeneratorService.generate(
      this.baseProperties.reduce((a, v) => ({ ...a, [v.alias]: v.value }), {})
    );

    this.generateState = 'init';
  }

  tour() {
    this.tourService.getTourByAlias("workflowGenerator").then(tour =>
      this.tourService.startTour(tour));
  }

  reset() {
    this.resetState = 'busy';
    this.wfGeneratorService.reset();
  }
}
