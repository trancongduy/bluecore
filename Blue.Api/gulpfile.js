/// <binding AfterBuild='copy-modules' />
"use strict";

/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    clean = require('gulp-clean'),
    glob = require('glob');

var paths = {
    host: {
        wwwroot: "./wwwroot/",
        wwwrootModules: "./wwwroot/modules/",
        modules: "./Modules/",
        moduleBin: "/bin/",
        bower: "./bower_components/"
    },
    dev: {
        modules: "../Modules/",
        moduleBin: "/bin/Debug/netcoreapp2.1"
    }
};

var modules = loadModules();

function loadModules() {
    var moduleManifestPaths,
        modules = [];

    moduleManifestPaths = glob.sync(paths.dev.modules + '*.*/module.json', {});

    moduleManifestPaths.forEach(function (moduleManifestPath) {
        var moduleManifest = require(moduleManifestPath);
        modules.push(moduleManifest);
    });
    return modules;
}

gulp.task('clean-module', function () {
    return gulp.src([paths.host.modules + '*', paths.host.wwwrootModules + '*'], { read: false })
        .pipe(clean());
});

gulp.task('copy-static', function () {
    modules.forEach(function (module) {
        gulp.src([paths.dev.modules + module.fullName + '/Views/**/*.*',
        paths.dev.modules + module.fullName + '/module.json'], { base: module.fullName })
            .pipe(gulp.dest(paths.host.modules + module.fullName));
        gulp.src(paths.dev.modules + module.fullName + '/wwwroot/**/*.*')
            .pipe(gulp.dest(paths.host.wwwrootModules + module.name));
    });
});

gulp.task('copy-modules', ['clean-module'], function () {
    modules.forEach(function (module) {
        if (!module.isBundledWithHost) {
            gulp.src(paths.dev.modules + module.fullName + paths.dev.moduleBin + '**/*.*')
                .pipe(gulp.dest(paths.host.modules + module.fullName + paths.host.moduleBin));
        }
    });
});

gulp.task('default', function () {
    // place code for your default task here
});