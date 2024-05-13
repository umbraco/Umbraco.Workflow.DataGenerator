const backofficePath = './src';

var argv = require('minimist')(process.argv.slice(2));

var outputPath = argv['output-path'];
if (!outputPath) {
  outputPath = require('./config.outputPath.js').outputPath;
}

export const paths = {
  js: [`${backofficePath}/**/*.ts`],
  views: [`${backofficePath}/**/*.html`],
  scss: `${backofficePath}/**/*.scss`,
  lang: `${backofficePath}/Lang/*.xml`,
  manifest: `${backofficePath}/**/package.manifest`,
  dest: outputPath,
};

export const config = {
  prod: argv["prod"] || false,
};
