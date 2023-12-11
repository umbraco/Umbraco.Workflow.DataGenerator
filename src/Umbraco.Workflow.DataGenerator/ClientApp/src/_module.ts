import { GeneratorDashboardController } from "./dashboard.controller";
import { GeneratorService } from "./service";

export const GeneratorModule = angular
  .module("umbraco.workflow.dataGenerator", [])
  .service(GeneratorService.serviceName, GeneratorService)
  .controller(
    GeneratorDashboardController.controllerName,
    GeneratorDashboardController
  ).name;
