define([
    "dojo",
    'dojo/_base/declare',
    'epi-cms/plugin-area/assets-pane',
    // Parent class
    'epi/_Module',
    // Commands
    'alloy/FullStackCreate/FullStackCreate'
], function (
    // Dojo
    dojo,
    declare,
    assetPaneEditorPluginArea,
    // Parent class
    _Module,
    FullStackCreate
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);
            assetPaneEditorPluginArea.add(FullStackCreate);
        }
    });
});