using Avalonia;
using Avalonia.Controls;
using BruTile;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Rendering.Skia;
using System;
using System.IO;
using System.Threading.Tasks;
using VextTile.Control.Mapsui;

namespace SampleApp.Views;

public partial class MainView : UserControl
{
    string _path = "files";
    IRenderedTileSource _tileSource;
    ILayer _tileLayer;

    public MainView()
    {
        InitializeComponent();

        MapRenderer.RegisterStyleRenderer(typeof(RenderedTileStyle), new RenderedTileStyleRenderer());

        var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

        _tileSource = new MapboxRenderedTileSource(stream);
        _tileLayer = new RenderedTileLayer(_tileSource)
        {
            Style = new RenderedTileStyle()
        };

        MapControl.Map.Layers.Add(_tileLayer);
        MapControl.Map.Navigator.RotationLock = false;

        //MapControl.Map.Navigator.RotateTo(0);
        MapControl.Map.RefreshData();

        var rotationSlider = RotationSlider.GetObservable(Slider.ValueProperty);
        //rotationSlider.Subscribe(value => { System.Diagnostics.Debug.WriteLine($"Rotation: {value}"); MapControl.Map.Navigator.RotateTo(value); }); 
    }
}
