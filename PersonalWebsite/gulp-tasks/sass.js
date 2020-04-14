var gulp = require("gulp");
var del = require("del");
var sass = require('gulp-sass');
sass.compiler = require('node-sass');
var autoprefixer = require('gulp-autoprefixer');
var minifyCss = require('gulp-minify-css');

gulp.task("clean:sass", function () {
    return del("./wwwroot/styles/**/*.css");
});

gulp.task("sass", function () {
    return gulp
        .src("./Styles/**/*.scss")
        .pipe(sass().on('error', sass.logError))
        .pipe(autoprefixer())
        .pipe(minifyCss())
        .pipe(gulp.dest('wwwroot/styles'));
});