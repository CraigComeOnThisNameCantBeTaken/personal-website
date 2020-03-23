var gulp = require("gulp");
var del = require("del");

var destination = "wwwroot/scripts/**/";

gulp.task("clean:typescript", function () {
    return del([destination + "*.js", destination + "*.map"])
});

// build is taken care of by MsBuild typescript nuget package