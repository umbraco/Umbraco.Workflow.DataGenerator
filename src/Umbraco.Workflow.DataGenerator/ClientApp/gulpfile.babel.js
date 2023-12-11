import gulp from 'gulp';

import { paths, config } from './gulp/config';
import { js } from './gulp/js';
import { scss } from './gulp/scss';
import { views } from './gulp/views';

function lang() {
  return gulp.src(paths.lang)
    .pipe(gulp.dest(paths.dest + '/Lang/'));
}

function manifest() {
  return gulp.src(paths.manifest)
    .pipe(gulp.dest(paths.dest));
}

function tour() {
  return gulp.src(paths.tour)
    .pipe(gulp.dest(paths.dest + '/tours/'));
}

// Entry points
export const build = gulp.task('build',
  gulp.series(
    done => {
      config.prod = true;
      done();
    },
    gulp.parallel(
      js,
      scss,
      views,
      lang,
      tour,
      manifest
    )));

export const dev = gulp.task('dev',
  gulp.series(
    gulp.parallel(
      js,
      scss,
      views,
      lang,
      tour,
      manifest
    ),
    done => {
      console.log('Watching for changes... Press Ctrl+C to exit.');

      gulp.watch(paths.js, gulp.series(js, views));
      gulp.watch(paths.scss, gulp.series(scss, views));
      gulp.watch(paths.views, gulp.series(views, js));
      gulp.watch(paths.lang, gulp.series(lang));
      gulp.watch(paths.manifest, gulp.series(manifest));
      gulp.watch(paths.tour, gulp.series(tour));
      done();
    }));
