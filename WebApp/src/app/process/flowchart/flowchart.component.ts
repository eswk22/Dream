import { Component, ViewEncapsulation, OnInit, Inject, ViewChild, ElementRef, AfterViewInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { ProcessService } from './../process.service';
import { FormGroup, AbstractControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { GridOptions } from "ag-grid/main";
import { AppState } from '../../app.service';
import { MatDialog } from '@angular/material';
import { PropertyComponent } from './properties/properties.component';
import { PropertyModel } from './../model/propertymodel';
import { SnotifyService } from 'ng-snotify';

declare const require: any;
declare var mxClient: any;
declare var mxObjectCodec: any;
declare var mxPanningManager: any;
declare var mxEvent: any;
declare var mxEditor: any;
declare var mxLog: any;
declare var mxUtils: any;
declare var mxCodec: any;
declare var mxResources: any;
declare var mxVertexHandler: any;
declare var mxGraphHandler: any;
declare var mxGuide: any;
declare var mxEdgeHandler: any;

declare var mxCell: any;
declare var mxGeometry: any;
declare var mxPoint: any;
declare var mxConstants: any;
declare var mxGraph: any;
declare var mxRubberband: any;
declare var mxPrintPreview: any;
declare var mxClipboard: any;
declare var mxUndoManager: any;
@Component({
	selector: 'flowchart',
	templateUrl: './flowchart.component.html',
	encapsulation: ViewEncapsulation.None,
	styles: [require('./flowchart.component.css')],
	providers: [
		ProcessService
	],
})

export class FlowChartComponent implements OnInit, AfterViewInit {
	public form: FormGroup;
	private gridOptions: GridOptions;
	public rowData: any[];
	private columnDefs: any[];
	private editor: any;
	private graph: any
	private history: any;
	private selectedObjectType: string = 'none';



	constructor( @Inject(FormBuilder) fb: FormBuilder, public appState: AppState,
		public _processManager: ProcessService, private _notification: SnotifyService,
		public dialog: MatDialog
	) {
		this.form = fb.group({
			name: ['', Validators.compose([Validators.required, Validators.minLength(4)])],
			objectid: [''],
			severityType: ['none'],
			txtcondition: [''],
			mergeType: ['anyone']
		});
	}

	ngOnInit() {
	}
	ngAfterViewInit() {
		var onGotAmdLoader = () => {
			// Load monaco
			//(<any>window).require(['mxClient'], () => {
			this.config();
			//});
		};

		// Load AMD loader if necessary
		if (!(<any>window).require) {
			var loaderScript = document.createElement('script');
			loaderScript.type = 'text/javascript';
			loaderScript.src = 'assets/editor/js/mxClient.js';
			loaderScript.addEventListener('load', onGotAmdLoader);
			document.body.appendChild(loaderScript);
		} else {
			onGotAmdLoader();
		}
	}



	//private configToolbar() {
	//	var buttons = ['group', 'ungroup', 'cut', 'copy', 'paste', 'delete', 'undo', 'redo', 'print'];
	//	for (var i = 0; i < buttons.length; i++) {
	//		var button = document.getElementById(buttons[i]);
	//		var factory = function (name, editor) {
	//			return function () {
	//				editor.execute(name);
	//			};
	//		};
	//		mxEvent.addListener(button, 'click', factory(buttons[i], this.editor));
	//	}
	//}



	private imglist: Array<string> = [];
	private GraphEditor = {};

	private config() {

		this.graph = new mxGraph();


		// Loads the default stylesheet into the graph
		var node = mxUtils.load('assets/editor/config/default-style.xml').getDocumentElement();
		var dec = new mxCodec(node.ownerDocument);
		dec.decode(node, this.graph.getStylesheet());

		// Sets the style to be used when an elbow edge is double clicked
		this.graph.alternateEdgeStyle = 'vertical';

		var graphPanel = document.getElementById('graph');
		// Initializes the graph as the DOM for the panel has now been created  
		this.graph.init(graphPanel);
		this.graph.setConnectable(true);
		this.graph.setDropEnabled(true);
		this.graph.setPanning(true);
		this.graph.setTooltips(true);
		this.graph.connectionHandler.setCreateTarget(true);
		this.graph.container.style.cursor = 'default';
		// Creates rubberband selection
		var rubberband = new mxRubberband(this.graph);
		this.insertVertexTemplate(document.getElementById('Test1'), this.graph, 'Start', '/resolve/jsp/model/images/symbols/start.png', 'symbol;image=/resolve/jsp/model/images/symbols/start.png', 30, 30, '');
		this.insertVertexTemplate(document.getElementById('Test2'), this.graph, 'End', '/resolve/jsp/model/images/symbols/end.png', 'symbol;image=/resolve/jsp/model/images/symbols/end.png', 30, 30, '');
		this.insertVertexTemplate(document.getElementById('Test3'), this.graph, 'Sub-Process', '/resolve/jsp/model/images/rectangle.gif', null, 100, 40, '');
		this.insertVertexTemplate(document.getElementById('Test4'), this.graph, 'Action', '/resolve/jsp/model/images/rounded.gif', 'rounded=1', 100, 40, '');
		this.insertVertexTemplate(document.getElementById('Test5'), this.graph, 'Precondition', '/resolve/jsp/model/images/rhombus.gif', 'rhombus', 60, 60, '');
		this.insertVertexTemplate(document.getElementById('Test6'), this.graph, 'Container', '/resolve/jsp/model/images/swimlane.gif', 'swimlane', 200, 200, '');
		this.insertVertexTemplate(document.getElementById('Test7'), this.graph, 'Text', '/resolve/jsp/model/images/text.gif', 'rounded=1', 100, 40, '');

		this.insertEdgeTemplate(document.getElementById('Test8'), this.graph, 'Straight Connector', '/resolve/jsp/model/images/straight.gif', 'straight;noEdgeStyle=1', 100, 100, '');
		this.insertEdgeTemplate(document.getElementById('Test9'), this.graph, 'Horizontal Connector', '/resolve/jsp/model/images/connect.gif', null, 100, 100, '');
		this.configHistory();
		this.definition();
	}

	private configHistory() {
		this.history = new mxUndoManager();
		//// Installs the command history after the initial graph
		//// has been created
		var listener = (sender, evt) => {
			this.history.undoableEditHappened(evt.getProperty('edit'));
		};

		this.graph.getModel().addListener(mxEvent.UNDO, listener);
		this.graph.getView().addListener(mxEvent.UNDO, listener);

		//// Keeps the selection in sync with the history
		var undoHandler = (sender, evt) => {
			var changes = evt.getProperty('edit').changes;
			this.graph.setSelectionCells(this.graph.getSelectionCellsForChanges(changes));
		};

		this.history.addListener(mxEvent.UNDO, undoHandler);
		this.history.addListener(mxEvent.REDO, undoHandler);
	}

	private definition() {
		mxClient.IS_SVG = true;
		mxUtils.getPrettyXmlOld = mxUtils.getPrettyXml;
		mxUtils.getPrettyXml = (node) => {
			return node.outerHTML || mxUtils.getPrettyXmlOld(node).replace('<#cdata-section/>', '');
		}
		mxEvent.disableContextMenu(document.body);

		// Makes the connection are smaller
		mxConstants.DEFAULT_HOTSPOT = 0.3;
		mxGraph.prototype.convertValueToString = function (cell) {
			var value = this.model.getValue(cell);

			if (value != null) {
				if (mxUtils.isNode(value)) {
					var lableValue = cell.getAttribute('label');

					if (lableValue != null) {
						return lableValue;
					}
					else {
						if (value.nodeName != 'Edge') {
							return value.nodeName;
						}
					}
				}
				else if (typeof (value.toString) == 'function') {
					return value.toString();
				}
			}
			return '';
		};
		mxGraphHandler.prototype.guidesEnabled = true;
		this.graph.labelChanged = (cell, newValue, trigger): void => {
			if (cell != null && newValue != null) {
				var elt = cell.value.cloneNode(true);
				elt.setAttribute('label', newValue);
				this.graph.model.setValue(cell, elt);
				this.form.controls["name"].setValue(newValue);
			}
		};
		this.graph.flipEdge = (edge) => {
			if (edge != null) {
				var state = this.graph.view.getState(edge);
				var style = (state != null) ? state.style : this.graph.getCellStyle(edge);

				if (style != null) {
					var elbow = mxUtils.getValue(style, mxConstants.STYLE_ELBOW,
						mxConstants.ELBOW_HORIZONTAL);
					var value = (elbow == mxConstants.ELBOW_HORIZONTAL) ?
						mxConstants.ELBOW_VERTICAL : mxConstants.ELBOW_HORIZONTAL;
					this.graph.setCellStyles(mxConstants.STYLE_ELBOW, value, [edge]);
				}
			}
		};

		//var onChangeHandler = (sender, evt) => {
		//	this.onSelectionChanged();
		//}
		var ondoubleClicked = (sender, evt) => {
			this.ondoubleClicked();
		}

		//		this.graph.getSelectionModel().addListener(mxEvent.CHANGE, onChangeHandler);
		this.graph.addListener(mxEvent.DOUBLE_CLICK, ondoubleClicked);
	}

	private ondoubleClicked() {
		let isSelected: boolean = !this.graph.isSelectionEmpty();
		if (isSelected) {
			var cell = this.graph.getSelectionCell();
			if (cell != null) {
				let property: PropertyModel = new PropertyModel();
				property.objectType = cell.edge == true ? 'edge' : 'vertex';
				if (cell.value) {
					property.name = cell.value.getAttribute('label');
					property.objectid = cell.value.getAttribute('objectid');
					property.mergeType = cell.value.getAttribute('merge');
					property.severityType = cell.value.getAttribute('severity');
					var params = '';
					if (cell.value.getElementsByTagName('inputParams')[0])
						params = cell.value.getElementsByTagName('inputParams')[0].innerHTML;
					property.input = this.extractParms(params);
					if (cell.value.getElementsByTagName('outputParams')[0])
						params = cell.value.getElementsByTagName('outputParams')[0].innerHTML;
					property.output = this.extractParms(params);
				}
				let dialogRef = this.dialog.open(PropertyComponent, { data: property });
				dialogRef.afterClosed().subscribe(result => {
					if (result) {
						var propertymodel = result[0];
						if (result[1]) {
							result[1].forEachNode(function (node) {
								propertymodel.input.push(node.data);
							});
						}
						if (result[2]) {
							result[2].forEachNode(function (node) {
								propertymodel.output.push(node.data);
							});
						}
						var selectedCell = this.graph.getSelectionCell();
						if (selectedCell.vertex == true) {
							var cell = selectedCell.value.cloneNode(true);
							if (cell != null) {
								cell.setAttribute('label', propertymodel.name);
								cell.setAttribute('merge', propertymodel.mergeType);
								cell.setAttribute('objectid', propertymodel.objectid);
								cell.getElementsByTagName('inputParams')[0].innerHTML = this.param2String(propertymodel.input);
								cell.getElementsByTagName('outputParams')[0].innerHTML = this.param2String(propertymodel.output);
								this.graph.model.setValue(selectedCell, cell);
							}
						}
						else {
							selectedCell.setAttribute('label', propertymodel.name);
							selectedCell.setAttribute('severity', propertymodel.severityType);
						}
					}
				});
			}
		}
	}
	//private onSelectionChanged() {
	//	let isSelected: boolean = !this.graph.isSelectionEmpty();
	//	console.log(isSelected);
	//	if (isSelected) {
	//		let name: string = '';
	//		let objectId: string = '';
	//		let severityType: string = 'none';
	//		let mergeType: string = 'anyone';
	//		var cell = this.graph.getSelectionCell();
	//		let params: string;
	//		if (cell != null) {
	//			if (cell.vertex) {
	//				this.selectedObjectType = cell.value.localName;
	//				name = cell.value.getAttribute('label');
	//				objectId = cell.value.getAttribute('objectid');
	//				mergeType = cell.value.getAttribute('merge');
	//				params = cell.value.getElementsByTagName('inputParams')[0].innerHTML;
	//			}
	//			if (cell.edge) {
	//				this.selectedObjectType = 'edge';
	//				//		severityType = cell.value.getAttribute('severity');
	//			}
	//			this.form.controls['name'].setValue(name);
	//			this.form.controls['objectid'].setValue(objectId);
	//		}
	//	}

	//}

	private param2String(params: Array<any>): string {
		let result: string = "";
		if (params) {
			result = 'CDATA [[ ' + JSON.stringify(params) + '  ]]';
		}
		return result;
	}
	private extractParms(params: string): any {
		if (params) {
			if (params.length > 12) {
				params = params.substring(9, params.length - 4);
				return JSON.parse(params);
			}
		}
		else
			return [];
	}


	private insertVertexTemplate(node, graph, name, icon, style, width, height, value) {
		var value = null;
		var img = name + "&" + style;
		this.imglist.push(img);
		var cells = [new mxCell((value != null) ? value : '', new mxGeometry(0, 0, width, height), style)];
		cells[0].vertex = true;

		var funct = function (graph, evt, target, x, y) {

			var styleLocal = style;
			var cells = [new mxCell((value != null) ? value : '', new mxGeometry(0, 0, width, height), styleLocal)];
			cells[0].vertex = true;
			cells = graph.getImportableCells(cells);

			if (cells.length > 0) {
				if (name == 'Action') {
					var actionObject = mxUtils.createXmlDocument().createElement(name);
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', name);
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('objectid', "");
					actionObject.setAttribute('merge', "anyone");
					actionObject.setAttribute('tooltip', "");
					actionObject.setAttribute('href', "");

					cells[0].setValue(actionObject);
					cells[0].style = "rounded;labelBackgroundColor=none;strokeColor=#A9A9A9;fillColor=#D3D3D3;gradientColor=white";
				}
				else if (name == 'Sub-Process') {
					var actionObject = mxUtils.createXmlDocument().createElement("Subprocess");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', name);
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('merge', "");
					actionObject.setAttribute('objectid', "");
					cells[0].setValue(actionObject);
					cells[0].style = "labelBackgroundColor=none;strokeColor=#A9A9A9;fillColor=#ADD8E6;gradientColor=white";
					//cells[0].geometry = new  mxGeometry(620, 40, 110, 32);
				}
				else if (name == 'Document') {
					var actionObject = mxUtils.createXmlDocument().createElement("Document");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', name);
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('merge', "anyone");
					actionObject.setAttribute('objectid', "");
					cells[0].setValue(actionObject);
					cells[0].style = "labelBackgroundColor=none;strokeColor=#A9A9A9;fillColor=#ADD8E6;gradientColor=white";
					//cells[0].geometry = new  mxGeometry(620, 40, 110, 32);
				}
				else if (name == 'Precondition') {
					var actionObject = mxUtils.createXmlDocument().createElement("Precondition");
					actionObject.setAttribute('label', "Precondition");
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('merge', "anyone");
					cells[0].setValue(actionObject);
					cells[0].style = "rhombus;labelBackgroundColor=none;strokeColor=#FFA500;fillColor=#FFA500;gradientColor=white";
					cells[0].geometry.width = 32;
					cells[0].geometry.height = 32;
				}
				else if (name == 'Question') {
					var actionObject = mxUtils.createXmlDocument().createElement("Question");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', "Question");
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('Id', "");
					cells[0].setValue(actionObject);
					cells[0].style = "rhombus;labelBackgroundColor=none;strokeColor=#FFA500;fillColor=#FFA500;gradientColor=white";
					cells[0].geometry.width = 32;
					cells[0].geometry.height = 32;
				}
				else if (name == 'Start') {
					var actionObject = mxUtils.createXmlDocument().createElement("Start");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', "Start");
					actionObject.setAttribute('name', "");
					cells[0].setValue(actionObject);
					cells[0].style = "symbol;image=/resolve/jsp/model/images/symbols/start.png";
				}
				else if (name == 'Root') {
					var actionObject = mxUtils.createXmlDocument().createElement("Start");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', "Root");
					actionObject.setAttribute('name', "");
					cells[0].setValue(actionObject);
					cells[0].style = "symbol;image=/resolve/jsp/model/images/symbols/start.png";
				}
				else if (name == 'End') {
					var actionObject = mxUtils.createXmlDocument().createElement("End");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', "End");
					actionObject.setAttribute('name', "");
					cells[0].setValue(actionObject);
					cells[0].style = "symbol;image=/resolve/jsp/model/images/symbols/end.png";
				}
				else if (name == 'Event') {
					var actionObject = mxUtils.createXmlDocument().createElement("Event");
					var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
					var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
					actionObject.appendChild(inputParams);
					actionObject.appendChild(outputParams);
					actionObject.setAttribute('label', "Event");
					actionObject.setAttribute('name', "");
					actionObject.setAttribute('merge', "anyone");
					cells[0].setValue(actionObject);
					cells[0].style = "symbol;image=/resolve/jsp/model/images/symbols/cancel_end.png";
				}
				else if (name == 'Container') {
					var actionObject = mxUtils.createXmlDocument().createElement("Container");
					actionObject.setAttribute('label', "Container");
					actionObject.setAttribute('name', "");
					cells[0].setValue(actionObject);
					cells[0].style = "swimlane;labelBackgroundColor=none;strokeColor=#A9A9A9;fillColor=#357EC7;gradientColor=white;fontColor=#FFFFFF";
				}
				else if (name == 'Text') {
					var actionObject = mxUtils.createXmlDocument().createElement("Text");
					actionObject.setAttribute('label', "Text");
					actionObject.setAttribute('name', "");
					cells[0].setValue(actionObject);
					cells[0].style = "labelBackgroundColor=none;strokeColor=none;fillColor=none;gradientColor=white;dashed=1";
				}
				else {
					//var style  = "";//cells[0].getStyle();

					if (this.imglist != null && this.imglist.length > 0) {
						for (var i = 0; i < this.imglist.length; i++) {
							var imgstring = this.imglist[i];
							var endIndex = imgstring.indexOf("&");
							var imgname = imgstring.substring(0, endIndex)

							if (imgname != null && imgname != '' && imgname == name) {
								styleLocal = imgstring.substring(endIndex + 1, imgstring.length);
								break;
							}
						}
					}

					if (styleLocal != null && styleLocal.indexOf('image') == 0) {
						var actionObject = mxUtils.createXmlDocument().createElement('Task');
						var inputParams = mxUtils.createXmlDocument().createElement('inputParams');
						var outputParams = mxUtils.createXmlDocument().createElement('outputParams');
						actionObject.appendChild(inputParams);
						actionObject.appendChild(outputParams);
						actionObject.setAttribute('label', name);
						actionObject.setAttribute('name', "");
						actionObject.setAttribute('merge', "anyone");
						actionObject.setAttribute('inputOutputDesc', "");
						actionObject.setAttribute('href', "");
						actionObject.style = styleLocal;
						cells[0].setValue(actionObject);
					}
				}

				var validDropTarget = (target != null) ?
					graph.isValidDropTarget(target, cells, evt) : false;
				var select = null;

				if (target != null &&
					!validDropTarget &&
					graph.getModel().getChildCount(target) == 0 &&
					graph.getModel().isVertex(target) == cells[0].vertex) {
					graph.getModel().setStyle(target, styleLocal);
					select = [target];
				}
				else {
					if (target != null && !validDropTarget) {
						target = null;
					}

					var pt = graph.getPointForEvent(evt);
					var ptLocal = pt;

					if (name == 'Task' || name == 'Runbook' || name == 'Event' || name == 'Container' || name == 'Text') {

						ptLocal.x -= graph.snap(width / 2 - 6);
						//pt.y -= graph.snap(height/2);
					}

					// Splits the target edge or inserts into target group
					if (graph.isSplitEnabled() &&
						graph.isSplitTarget(target, cells, evt)) {
						graph.splitEdge(target, cells, null, ptLocal.x, pt.y);
						select = cells;
					}
					else {
						cells = graph.getImportableCells(cells);

						if (cells.length > 0) {
							select = graph.importCells(cells, pt.x + width / 2, pt.y, target);
						}
					}
				}

				if (select != null && select.length > 0) {
					graph.scrollCellToVisible(select[0]);
					graph.setSelectionCells(select);
				}
			}
		};




		var installDrag = function (expandedNode, graph) {
			if (node != null) {
				var dragPreview = document.createElement('div');
				dragPreview.style.border = 'dashed black 1px';
				dragPreview.style.width = width + 'px';
				dragPreview.style.height = height + 'px';

				mxUtils.makeDraggable(node, graph, funct, dragPreview, 0, 0,
					graph.autoscroll, true);


			}
		};


		installDrag(node, graph);

		return node;
	};


	private insertEdgeTemplate(node, graph, name, icon, style, width, height, value) {
		var cells = [new mxCell((value != null) ? value : '', new mxGeometry(0, 0, width, height), style)];
		cells[0].geometry.setTerminalPoint(new mxPoint(0, height), true);
		cells[0].geometry.setTerminalPoint(new mxPoint(width, 0), false);
		cells[0].edge = true;

		var funct = function (graph, evt, target) {
			cells = graph.getImportableCells(cells);

			if (cells.length > 0) {
				if (name == 'Straight Connector' || name == 'Horizontal Connector' || name == 'Vertical Connector'
					|| name == 'Answer' || name == 'Answer (Horizontal)') {
					var edgeObject = mxUtils.createXmlDocument().createElement("Edge");
					edgeObject.setAttribute('label', "");
					edgeObject.setAttribute('name', "");
					edgeObject.setAttribute('severity', "none");
					cells[0].setValue(edgeObject);
				}

				var validDropTarget = (target != null) ?
					graph.isValidDropTarget(target, cells, evt) : false;
				var select = null;

				if (target != null &&
					!validDropTarget) {
					target = null;
				}

				var pt = graph.getPointForEvent(evt);
				var scale = graph.view.scale;

				pt.x -= graph.snap(width / 2);
				pt.y -= graph.snap(height / 2);

				select = graph.importCells(cells, pt.x, pt.y, target);

				// Uses this new cell as a template for all new edges
				//this.GraphEditor.edgeTemplate = select[0];

				graph.scrollCellToVisible(select[0]);
				graph.setSelectionCells(select);
			}
		};

		// Small hack to install the drag listener on the node's DOM element
		// after it has been created. The DOM node does not exist if the parent
		// is not expanded.
		var installDrag = function (expandedNode) {
			if (node != null) {
				// Creates the element that is being shown while the drag is in progress
				var dragPreview = document.createElement('div');
				dragPreview.style.border = 'dashed black 1px';
				dragPreview.style.width = width + 'px';
				dragPreview.style.height = height + 'px';

				mxUtils.makeDraggable(node, graph, funct, dragPreview, -width / 2, -height / 2,
					graph.autoscroll, true);
			}
		};


		installDrag(node);


		return node;
	};

	private readflowChart() {
		var enc = new mxCodec(mxUtils.createXmlDocument());
		//enc.encodeDefaults = true;
		var node = enc.encode(this.graph.getModel());
		var xml = mxUtils.getPrettyXml(node);
		console.log(xml);
	}

	private executeToolbar(action: string) {
		switch (action) {
			case 'print':
				var preview = new mxPrintPreview(this.graph, 1);
				preview.open();
				break;
			case 'cut':
				mxClipboard.cut(this.graph);
				break;
			case 'copy':
				mxClipboard.copy(this.graph);
				break;
			case 'paste':
				mxClipboard.paste(this.graph);
				break;
			case 'undo':
				this.history.undo();
				break;
			case 'redo':
				this.history.redo();
				break;
			case 'delete':
				this.graph.removeCells();
				break;
			case 'zoomactual':
				this.graph.zoomActual();
				break;
			case 'zoomin':
				this.graph.zoomIn();
				break;
			case 'zoomout':
				this.graph.zoomOut();
				break;
		}

	}
}