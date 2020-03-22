/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp");
var del = require("del");
var sass = require('gulp-sass');
sass.compiler = require('node-sass');

var paths = {
    scripts: ["scripts/**/*.js", "scripts/**/*.ts", "scripts/**/*.map"],
    sass: ["./wwwroot/styles/**/*.scss"]
};






gulp.task("clean:scripts", function () {
    return del(["wwwroot/scripts/**/*.js"]);
});

gulp.task("clean:sass", function () {
    return del(["wwwroot/styles/**/*.css"]);
});

gulp.task('clean', gulp.series('clean:scripts', 'clean:sass'));







gulp.task("scripts", async function () {
    gulp.src(paths.scripts).pipe(gulp.dest("wwwroot/scripts"))
});

gulp.task("sass", function () {
    return gulp.src(paths.sass)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('wwwroot/styles'))
});

gulp.task('default', gulp.series('scripts', 'sass'));
