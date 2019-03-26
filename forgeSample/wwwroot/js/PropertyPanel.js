// *******************************************
// Custom Property Panel Extension
// *******************************************
function CustomPropertyPanelExtension(viewer, options) {
    Autodesk.Viewing.Extension.call(this, viewer, options);

    this.viewer = viewer;
    this.options = options;
    this.panel = null;
}

CustomPropertyPanelExtension.prototype = Object.create(Autodesk.Viewing.Extension.prototype);
CustomPropertyPanelExtension.prototype.constructor = CustomPropertyPanelExtension;

CustomPropertyPanelExtension.prototype.load = function () {
    this.panel = new CustomPropertyPanel(this.viewer, this.options);
    this.viewer.setPropertyPanel(this.panel);
    return true;
};

CustomPropertyPanelExtension.prototype.unload = function () {
    this.viewer.setPropertyPanel(null);
    this.panel = null;
    return true;
};

Autodesk.Viewing.theExtensionManager.registerExtension('CustomPropertyPanelExtension', CustomPropertyPanelExtension);

// *******************************************
// Custom Property Panel
// *******************************************
function CustomPropertyPanel(viewer, options) {
    this.viewer = viewer; 
    this.options = options; 
    this.nodeId = -1; // dbId of the current element showing properties
    Autodesk.Viewing.Extensions.ViewerPropertyPanel.call(this, this.viewer);
}
CustomPropertyPanel.prototype = Object.create(Autodesk.Viewing.Extensions.ViewerPropertyPanel.prototype);
CustomPropertyPanel.prototype.constructor = CustomPropertyPanel;

CustomPropertyPanel.prototype.setProperties = function (properties, options) {
    Autodesk.Viewing.Extensions.ViewerPropertyPanel.prototype.setProperties.call(this, properties, options);

    // add your custom properties here
    // for example, let's show the dbId and externalId
    var _this = this;
    // dbId is right here as nodeId
    this.addProperty('dbId', this.nodeId, 'Custom Properties');
    // externalId is under all properties, let's get it!
    this.viewer.getProperties(this.nodeId, function(props){
        _this.addProperty('externalId', props.externalId, 'Custom Properties');
    })
}

CustomPropertyPanel.prototype.setNodeProperties = function (nodeId) {
    Autodesk.Viewing.Extensions.ViewerPropertyPanel.prototype.setNodeProperties.call(this, nodeId);
    this.nodeId = nodeId;
};