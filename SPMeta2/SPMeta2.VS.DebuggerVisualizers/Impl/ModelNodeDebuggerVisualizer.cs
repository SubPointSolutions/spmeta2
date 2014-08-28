using Microsoft.VisualStudio.DebuggerVisualizers;
using SPMeta2.Models;
using SPMeta2.VS.DebuggerVisualizers.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: DebuggerVisualizer(
    typeof(ModelNodeDebuggerVisualizer),
    typeof(VisualizerObjectSource),
    Target = typeof(ModelNode),
    Description = "SPMeta2 Model Node Visualizer")]
namespace SPMeta2.VS.DebuggerVisualizers.Impl
{
    public class ModelNodeDebuggerVisualizer : DialogDebuggerVisualizer
    {
        override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var modelNode = (ModelNode)objectProvider.GetObject();

            var form = new Form();

            form.ClientSize = new Size(800, 600);
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            var treeView = new TreeView();
            treeView.Parent = form;
            treeView.Dock = DockStyle.Fill;

            InitNodes(treeView.Nodes, modelNode);
            treeView.ExpandAll();

            windowService.ShowDialog(form);
        }

        private void InitNodes(TreeNodeCollection treeNodeCollection, ModelNode modelNode)
        {
            var nodeValue = modelNode.Value;

            var newTreeNode = treeNodeCollection.Add(nodeValue == null ? "NULL" : nodeValue.ToString());

            foreach (var childNode in modelNode.ChildModels)
            {
                InitNodes(newTreeNode.Nodes, childNode);
            }
        }


    }
}
