webpackJsonpac__name_([1],{

/***/ 444:
/*!*****************************************************************************************************************!*\
  !*** ./~/awesome-typescript-loader/dist.babel/entry.js!./~/angular2-template-loader!./src/app/+detail/index.ts ***!
  \*****************************************************************************************************************/
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	function __export(m) {
	    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
	}
	var detail_component_1 = __webpack_require__(/*! ./detail.component */ 805);
	exports.Detail = detail_component_1.Detail;
	var index_component_1 = __webpack_require__(/*! ./index.component */ 806);
	exports.Index = index_component_1.Index;
	console.log('`Detail` bundle loaded asynchronously');
	// Must be exported for WebpackAsyncRoute
	__export(__webpack_require__(/*! ./routes */ 807));


/***/ },

/***/ 805:
/*!*********************************************!*\
  !*** ./src/app/+detail/detail.component.ts ***!
  \*********************************************/
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var core_1 = __webpack_require__(/*! @angular/core */ 1);
	var router_1 = __webpack_require__(/*! @angular/router */ 127);
	var Detail = (function () {
	    function Detail() {
	    }
	    Detail.prototype.ngOnInit = function () {
	        console.log('hello `Detail` component');
	    };
	    Detail = __decorate([
	        core_1.Component({
	            selector: 'detail',
	            directives: router_1.ROUTER_DIRECTIVES.slice(),
	            template: "\n    <router-outlet></router-outlet>\n  "
	        }), 
	        __metadata('design:paramtypes', [])
	    ], Detail);
	    return Detail;
	}());
	exports.Detail = Detail;


/***/ },

/***/ 806:
/*!********************************************!*\
  !*** ./src/app/+detail/index.component.ts ***!
  \********************************************/
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var core_1 = __webpack_require__(/*! @angular/core */ 1);
	var Index = (function () {
	    function Index() {
	    }
	    Index.prototype.ngOnInit = function () {
	        console.log('hello `Index` component');
	    };
	    Index = __decorate([
	        core_1.Component({
	            selector: 'index',
	            styles: ["\n    md-card{\n      margin: 25px;\n    }\n  "],
	            template: "\n    <md-card>Hello from Index</md-card>\n  "
	        }), 
	        __metadata('design:paramtypes', [])
	    ], Index);
	    return Index;
	}());
	exports.Index = Index;


/***/ },

/***/ 807:
/*!***********************************!*\
  !*** ./src/app/+detail/routes.ts ***!
  \***********************************/
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var detail_component_1 = __webpack_require__(/*! ./detail.component */ 805);
	var index_component_1 = __webpack_require__(/*! ./index.component */ 806);
	// async components must be named routes for WebpackAsyncRoute
	exports.routes = {
	    path: 'detail', component: detail_component_1.Detail,
	    children: [
	        { path: '', component: index_component_1.Index }
	    ]
	};


/***/ }

});
//# sourceMappingURL=1.map