// the normal typescript build process is fine to actually build my typescript however
// a module bundler is requested to use the import export syntax since browsers dont support commonjs - hence webpack.
// todo: determine if the other gulp tasks can instead be done by webpack

const gulp = require('gulp');
const webpack = require('webpack');
var config = require('./../webpack.config.js');
//var config = require(path.join('../..', 'webpack.config.es6.js'));

gulp.task('bundle-javascript', async () => {
    return webpack(config).run((err, stats) => {
        if (err)
            console.log(err)
    });
});