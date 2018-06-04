webpackHotUpdateac__name_(1,{

/***/ 709:
/***/ function(module, exports, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(exports, "a", function() { return BarrelComponent; });

/*
 * We're loading this component asynchronously
 * We are using some magic with es6-promise-loader that will wrap the module with a Promise
 * see https://github.com/gdi2290/es6-promise-loader for more info
 */
console.log('`Barrel` component loaded asynchronously');
var BarrelComponent = (function () {
    function BarrelComponent() {
    }
    BarrelComponent.prototype.ngOnInit = function () {
        console.log('hello `Barrel` component');
    };
    BarrelComponent = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_1" /* Component */])({
            selector: 'barrel',
            template: "\n    <h1>Hello from Barrel</h1>\n    <span>\n      <a [routerLink]=\" ['./child-barrel'] \">\n        Child Barrel\n      </a>\n    </span>\n    <router-outlet></router-outlet>\n  ",
        }), 
        __metadata('design:paramtypes', [])
    ], BarrelComponent);
    return BarrelComponent;
}());


/***/ }

})
//# sourceMappingURL=1.00a7506ad44ca6b74e5e.hot-update.js.map