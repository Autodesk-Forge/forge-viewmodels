class ImprovedVisualExtension extends Autodesk.Viewing.Extension {
    constructor(viewer, options) {
        super(viewer, options);
        this._group = null;
        this._button = null;
    }

    load() {
        console.log('ImprovedVisual has been loaded');
        return true;
    }

    unload() {
        // Clean our UI elements if we added any
        if (this._group) {
            this._group.removeControl(this._button);
            if (this._group.getNumberOfControls() === 0) {
                this.viewer.toolbar.removeControl(this._group);
            }
        }
        console.log('ImprovedVisual has been unloaded');
        return true;
    }

    onToolbarCreated() {
        // Create a new toolbar group if it doesn't exist
        this._group = this.viewer.toolbar.getControl('customToolbar');
        if (!this._group) {
            this._group = new Autodesk.Viewing.UI.ControlGroup('customToolbar');
            this.viewer.toolbar.addControl(this._group);
        }

        // Add a new button to the toolbar group
        this._button = new Autodesk.Viewing.UI.Button('ImprovedVisualButton');
        this._button.onClick = (ev) => {
            // Execute an action here

            //change lights
            this.viewer.setLightPreset(1);
            // apply shadows and angle
            this.viewer.impl.toggleShadows(true);
            this.viewer.impl.setShadowLightDirection(new THREE.Vector3(-1, 2, 1));
            // adjust perspective
            this.viewer.impl.renderer().setAOOptions(10, 0.1);
            this.viewer.navigation.toPerspective();
            this.viewer.setFOV(25);
        };
        this._button.setToolTip('Enable improved visual');
        this._button.addClass('ImprovedVisualIcon');
        this._group.addControl(this._button);
    }
}

Autodesk.Viewing.theExtensionManager.registerExtension('ImprovedVisualExtension', ImprovedVisualExtension);
