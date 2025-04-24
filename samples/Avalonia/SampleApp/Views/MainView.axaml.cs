using Avalonia;
using Avalonia.Controls;
using Mapsui.Rendering.Skia;
using Mapsui.Tiling;
using System.IO;
using VexTile.Renderer.Common;
using VextTile.Control.Mapsui;

namespace SampleApp.Views;

public partial class MainView : UserControl
{
    string _path = "files";

    public MainView()
    {
        InitializeComponent();

        MapRenderer.RegisterStyleRenderer(typeof(RenderedTileStyle), new RenderedTileStyleRenderer());

        var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

        var tileSource = new MapboxRenderedTileSource(stream);
        var tileLayer = new RenderedTileLayer(tileSource, tileInformation: new TileInformation { Border = true, Text = true });
        var symbolsLayer = new RenderedSymbolsLayer(tileSource);

        MapControl.Map.Layers.Add(tileLayer);
        MapControl.Map.Layers.Add(symbolsLayer);

        MapControl.Map.Navigator.RotationLock = false;

        //MapControl.Map.Navigator.RotateTo(0);
        MapControl.Map.RefreshData();

        var rotationSlider = RotationSlider.GetObservable(Slider.ValueProperty);
        //rotationSlider.Subscribe(value => { System.Diagnostics.Debug.WriteLine($"Rotation: {value}"); MapControl.Map.Navigator.RotateTo(value); }); 
    }
}
