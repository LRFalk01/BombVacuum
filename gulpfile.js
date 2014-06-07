var gulp = require('gulp');
var usemin = require('gulp-usemin');
var uglify = require('gulp-uglify');
var minifyHtml = require('gulp-minify-html');
var minifyCss = require('gulp-minify-css');
var rev = require('gulp-rev');

gulp.task('default', function(){
    gulp.start('usemin');
});

gulp.task('usemin', function() {
    gulp.src('./bin/Release/*.html')
        .pipe(usemin({
            css: [minifyCss(), 'concat', rev()],
            //html: [minifyHtml({ empty: true, comments: true })],
            js: [uglify(), rev()]
        }))
        .pipe(gulp.dest('./bin/Release'));
});

//gulp.task('minify-html', function () {
//    var opts = { comments: true, empty: true };

//    gulp.src('./bin/Release/assets/app**.html')
//        .pipe(minifyHtml(opts))
//        .pipe(gulp.dest('./dist/'));
//});