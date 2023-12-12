import { GeneratorDashboardController } from "./dashboard.controller";
import { GeneratorService } from "./service";

export const GeneratorModule = angular
  .module("workflow.dataGenerator.components", [])
  .service(GeneratorService.serviceName, GeneratorService)
  .controller(
    GeneratorDashboardController.controllerName,
    GeneratorDashboardController
  ).name;
