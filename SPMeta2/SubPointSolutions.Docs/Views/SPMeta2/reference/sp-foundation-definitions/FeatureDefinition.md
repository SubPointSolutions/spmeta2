Feature activation/deactivation is enabled via FeatureDefinition object.

Both CSOM/SSOM object models are supported, except webapp/farm features could be deployed only with SSOM object model.

Provision looks up feature by ID and acts according feature definition properties Enable/ForceActivate.

Enable suggest to enable feature if it was not activated, but skip if it is activates. ForceActivate flag allows to force activate feature despite of current state.  You can deploy either single feature or a set using AddWebFeature() extension method.