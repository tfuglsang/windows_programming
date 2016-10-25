using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ClassDiagram.Annotations;

namespace ClassDiagram.Helpers
{
    class MyCustomAdorner : Adorner
    {
        // THIS CLASS IS CURRENTLY UNUSED
        public MyCustomAdorner([NotNull] UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);



            var right = adornedElementRect.Right;
            var left = adornedElementRect.Left;
            var top = adornedElementRect.Top;
            var bottom = adornedElementRect.Bottom;

            var segments = new[]
            {
              new LineSegment(new Point(left, top), true),
              new LineSegment(new Point(right, bottom), true),
              new LineSegment(new Point(right, top), true)
           };

            var figure = new PathFigure(new Point(left, top), segments, true);
            var geometry = new PathGeometry(new[] { figure });
            drawingContext.DrawGeometry(Brushes.Red, null, geometry);
        }
    }
}