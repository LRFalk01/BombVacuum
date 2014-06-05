var gulp = require('gulp');
var usemin = require('gulp-usemin');
var uglify = require('gulp-uglify');

var minifyCss = require('gulp-minify-css');
var rev = require('gulp-rev');

gulp.task('default', function(){
        gulp.start('usemin');
});

gulp.task('usemin', function() {
    gulp.src('./*.html')
        .pipe(usemin({
            css: [minifyCss(), 'concat', rev()],

            js: [uglify(), rev()]
        }))
        .pipe(gulp.dest('assets/'));
});
