import { GeneratorModule } from './_module';

const name = "workflow.dataGenerator";

angular.module(name, [GeneratorModule]);

angular.module('workflow').requires.push(name);
