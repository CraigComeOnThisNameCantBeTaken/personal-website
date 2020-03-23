// to be run manually as needed

var gulp = require("gulp");

gulp.task("jquery", function () {
    return gulp
        .src("node_modules/jquery/dist/jquery.min.js")
        .pipe(gulp.dest('wwwroot/lib'));
});