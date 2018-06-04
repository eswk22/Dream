webpackJsonpac__name_([1],{

/***/ 705:
/***/ function(module, exports, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__barrel_module__ = __webpack_require__(715);
Object.defineProperty(exports, "__esModule", { value: true });
/* harmony reexport (binding) */ __webpack_require__.d(exports, "BarrelModule", function() { return __WEBPACK_IMPORTED_MODULE_0__barrel_module__["a"]; });



/***/ },

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


/***/ },

/***/ 715:
/***/ function(module, exports, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_common__ = __webpack_require__(113);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(416);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__(254);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__barrel_routes__ = __webpack_require__(716);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__barrel_component__ = __webpack_require__(709);
/* harmony export (binding) */ __webpack_require__.d(exports, "a", function() { return BarrelModule; });






console.log('`Barrel` bundle loaded asynchronously');
var BarrelModule = (function () {
    function BarrelModule() {
    }
    BarrelModule.routes = __WEBPACK_IMPORTED_MODULE_4__barrel_routes__["a" /* routes */];
    BarrelModule = __decorate([
        __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_2__angular_core__["i" /* NgModule */])({
            declarations: [
                // Components / Directives/ Pipes
                __WEBPACK_IMPORTED_MODULE_5__barrel_component__["a" /* BarrelComponent */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_0__angular_common__["b" /* CommonModule */],
                __WEBPACK_IMPORTED_MODULE_1__angular_forms__["a" /* FormsModule */],
                __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* RouterModule */].forChild(__WEBPACK_IMPORTED_MODULE_4__barrel_routes__["a" /* routes */]),
            ],
        }), 
        __metadata('design:paramtypes', [])
    ], BarrelModule);
    return BarrelModule;
}());


/***/ },

/***/ 716:
/***/ function(module, exports, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__barrel_component__ = __webpack_require__(709);
/* harmony export (binding) */ __webpack_require__.d(exports, "a", function() { return routes; });

var routes = [
    { path: '', children: [
            { path: '', component: __WEBPACK_IMPORTED_MODULE_0__barrel_component__["a" /* BarrelComponent */] },
            { path: 'child-barrel', loadChildren: function () { return __webpack_require__.e/* import() */(5).then(__webpack_require__.bind(null, 714)).then(function (module) { return module['ChildBarrelModule']; }); } }
        ] },
];


/***/ }

});
//# sourceMappingURL=1.chunk.js.map