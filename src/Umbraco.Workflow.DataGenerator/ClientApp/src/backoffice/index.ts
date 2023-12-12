import { GeneratorModule } from './_module';

const name = "workflow.dataGenerator";

angular.module(name, [GeneratorModule]);

angular.module('umbraco').requires.push(name);
