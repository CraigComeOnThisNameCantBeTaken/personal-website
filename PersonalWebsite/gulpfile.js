/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp");
var del = require("del");
var sass = require('gulp-sass');
sass.compiler = require('node-sass');

var containingPaths = {
    scripts: "./wwwroot/scripts/**/",
    sass: "./wwwroot/styles/**/"
};

gulp.task("clean:scripts", function () {
    return del([containingPaths.scripts + "*.js", containingPaths.scripts + "*.map"])
});

gulp.task("clean:sass", function () {
    return del(containingPaths.sass + "*.css");
});

gulp.task('clean', gulp.series('clean:scripts', 'clean:sass'));

gulp.task("sass", function () {
    return gulp.src(containingPaths.sass + "*.scss")
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('wwwroot/styles'))
});

gulp.task('default', gulp.series('sass'));
