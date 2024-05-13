type ObjectArray = Array<Record<string, any>>;

export class GeneratorDashboardController {
  public static controllerName = "Workflow.GeneratorDashboard.Controller";

  hasInstalledWorkflowData = false;
  generateState = 'init';
  resetState = 'init';

  baseProperties: ObjectArray = [
    {
      alias: "groupCount",
      view: "integer",
      value: 2,
      min: 2,
    },
    {
      alias: "userCount",
      view: "integer",
      value: 2,
      min: 2,
    },
    {
      alias: "groupsPerWorkflow",
      view: "integer",
      value: 0,
      min: 0,
    },
    {
      alias: "usersPerGroup",
      view: "integer",
      value: 0,
      min: 0,
    },
  ];
  
  tours: Array<Record<string, any>> = [];

  constructor(private wfGeneratorService, private tourService, private overlayService, private localizationService, private notificationsService) {
    this.baseProperties.forEach(async prop => {
      const results = await this.localizationService.localizeMany([`workflowDataGenerator${prop.alias}`, `workflowDataGenerator${prop.alias}Description`]);
      prop.label = results[0];
      if (results[1].startsWith('[')) return;
      prop.description = results[1];
    });
  }

  async $onInit() {
    this.hasInstalledWorkflowData = await this.wfGeneratorService.status();
    if (this.hasInstalledWorkflowData) {
      this.getTours();
    }
  }

  async generate() {
    this.generateState = 'busy';
    this.hasInstalledWorkflowData = await this.wfGeneratorService.generate(
      this.baseProperties.reduce((a, v) => ({ ...a, [v.alias]: v.value }), {})
    );    
    this.generateState = 'init';

    if (this.hasInstalledWorkflowData) {
      this.tours.length === 0 ? this.getTours() : null;
      this.showNotificationOverlay();
    } else {
      this.notificationsService.error("Unable to generate Workflow data");
    }
  }

  async getTours() {
    const { tourData } = await import('./generator.tour');
    this.tours = tourData;
  }

  async showNotificationOverlay() {
    const [title, content] = [...(await this.localizationService.localizeMany(['workflowDataGenerator_postInstallTitle', 'workflowDataGenerato_rpostInstallContent']))];

    const overlayModel = {
      title,
      content,
      view: `${Umbraco.Sys.ServerVariables.UmbracoWorkflow.viewsPath}overlays/html.overlay.html`,
      submitButtonLabelKey: 'workflowDataGenerator_startTour',
      submitButtonStyle: 'info',
      submit: () => {
        this.overlayService.close();
        this.startTour(this.tours[0]);
      },
      close: () => this.overlayService.close(),
    }

    this.overlayService.open(overlayModel);
  }

  startTour(tour) {
    this.tourService.startTour(tour);
  }

  reset() {
    this.resetState = 'busy';
    this.wfGeneratorService.reset().then(() => {
      this.hasInstalledWorkflowData = false;
      this.resetState = 'init';
    });
  }
}
