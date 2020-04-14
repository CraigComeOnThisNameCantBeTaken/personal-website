/// <binding BeforeBuild='default' Clean='clean' ProjectOpened='watch' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp");
var requireDir = require('require-dir');

requireDir('./gulp-tasks');

gulp.task('clean', gulp.series('clean:typescript', 'clean:sass'));

gulp.task('watch', function () {
    gulp.watch('Styles/*.scss', gulp.series('sass'));
    gulp.watch('Scripts/*.js', gulp.series('bundle-javascript'));
});

gulp.task('default', gulp.series('sass', 'bundle-javascript'));

//gulp.task('default', ['scripts', 'sass'], function () {
//    gulp.watch('src/js/**/*.js', ['scripts']);
//    gulp.watch('src/sass/**/*.{sass,scss}', ['sass']);
//});