﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Delta.Input.States;

namespace Delta.UI
{
    public abstract class Control : EntityCollection<Control>
    {
        internal Rectangle _cullRectangle = Rectangle.Empty;
        internal Vector2 _renderPosition = Vector2.Zero;
        internal Vector2 _renderSize = Vector2.Zero;
        internal Vector2 _renderBorderSize = Vector2.Zero;
        internal Vector2 _innerRenderPosition = Vector2.Zero;
        internal Vector2 _innerRenderSize = Vector2.Zero;
        internal Color _currentColor = Color.White;
        internal Color _currentBorderColor = Color.White;

        protected internal Rectangle RenderArea { get; internal set; }
        protected internal Rectangle InnerArea { get; internal set; }

        Control _parent = null;
        public Control Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPositionChanged();
                }
            }
        }

        Point _position = Point.Zero;
        public Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        bool _isPressed = false;
        public bool IsPressed
        {
            get { return _isPressed; }
            internal set
            {
                if (_isPressed != value)
                {
                    _isPressed = value;
                    if (value)
                        OnPressed();
                    else
                        OnReleased();
                    NeedsHeavyUpdate = true;
                }
            }
        }

        bool _isFocusable = true;
        public bool IsFocusable
        {
            get { return _isFocusable; }
            set
            {
                if (_isFocusable != value)
                {
                    _isFocusable = value;
                    if (!value && IsFocused)
                        IsFocused = false;
                }
            }
        }

        bool _isFocused = false;
        public bool IsFocused
        {
            get { return _isFocused; }
            internal set
            {
                if (_isFocused != value)
                {
                    _isFocused = value;
                    if (value)
                        OnGotFocus();
                    else
                        OnLostFocus();
                    NeedsHeavyUpdate = true;
                }
            }
        }

        bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            internal set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

#if WINDOWS
        bool _mouseIsInside = false;
        public bool MouseIsInside
        {
            get { return _mouseIsInside; }
            internal set
            {
                if (_mouseIsInside != value)
                {
                    _mouseIsInside = value;
                    if (value)
                        OnMouseEnter();
                    else
                        OnMouseLeave();
                }
            }
        }
#endif

        Color _backColor = Color.SeaGreen;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _borderColor = Color.DarkSeaGreen;
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _disabledColor = Color.Gray;
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set
            {
                if (_disabledColor != value)
                {
                    _disabledColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _disabledBorderColor = Color.DarkGray;
        public Color DisabledBorderColor
        {
            get { return _disabledBorderColor; }
            set
            {
                if (_disabledBorderColor != value)
                {
                    _disabledBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }


        Color _highlightedColor = Color.DarkKhaki;
        public Color HighlightedColor
        {
            get { return _highlightedColor; }
            set
            {
                if (_highlightedColor != value)
                {
                    _highlightedColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _highlightedBorderColor = Color.Khaki;
        public Color HighlightedBorderColor
        {
            get { return _highlightedBorderColor; }
            set
            {
                if (_highlightedBorderColor != value)
                {
                    _highlightedBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _pressedColor = Color.DarkViolet;
        public Color PressedColor
        {
            get { return _pressedColor; }
            set
            {
                if (_pressedColor != value)
                {
                    _pressedColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _pressedBorderColor = Color.Violet;
        public Color PressedBorderColor
        {
            get { return _pressedBorderColor; }
            set
            {
                if (_pressedBorderColor != value)
                {
                    _pressedBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _focusedColor = Color.DarkSlateBlue;
        public Color FocusedColor
        {
            get { return _focusedColor; }
            set
            {
                if (_focusedColor != value)
                {
                    _focusedColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _focusedBorderColor = Color.SlateBlue;
        public Color FocusedBorderColor
        {
            get { return _focusedBorderColor; }
            set
            {
                if (_focusedBorderColor != value)
                {
                    _focusedBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Point _size = new Point(25, 25);
        public Point Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                }
            }
        }

        Point _borderSize = new Point(1, 1);
        public Point BorderSize
        {
            get { return _borderSize; }
            set
            {
                if (_borderSize != value)
                {
                    _borderSize = value;
                    OnBorderSizeChanged();
                }
            }
        }

        public Control()
            : base()
        {
        }

        public override void Add(Control item)
        {
            base.Add(item);
            item.Parent = this;
        }

        public override void Remove(Control item)
        {
            base.Remove(item);
            item.Parent = null;
        }

        protected internal override void HeavyUpdate(DeltaTime time)
        {
            base.HeavyUpdate(time);
            UpdateRenderBorderSize();
            UpdateRenderPosition();
            UpdateInnerRenderPosition();
            UpdateRenderSize();
            UpdateInnerRenderSize();
            UpdateRenderArea();
            UpdateInnerArea();
            UpdateColors();
            UpdateCullRectangle();
            if (Children.Count != 0)
                foreach (Control control in Children)
                    control.NeedsHeavyUpdate = true;
        }

        internal virtual void UpdateRenderBorderSize()
        {
            _renderBorderSize = new Vector2(_borderSize.X, _borderSize.Y);
        }

        internal virtual void UpdateRenderPosition()
        {
            _renderPosition = new Vector2(_position.X, _position.Y);
            if (Parent != null)
                _renderPosition += Parent._renderPosition;
        }

        internal virtual void UpdateInnerRenderPosition()
        {
            _innerRenderPosition = _renderPosition + _renderBorderSize;
        }

        internal virtual void UpdateRenderSize()
        {
            _renderSize = new Vector2(Size.X, Size.Y);
        }

        internal virtual void UpdateInnerRenderSize()
        {
            _innerRenderSize = _renderSize - _renderBorderSize * 2;
        }

        internal virtual void UpdateRenderArea()
        {
            RenderArea = new Rectangle(
                (int)_renderPosition.X, 
                (int)_renderPosition.Y, 
                (int)_renderSize.X, 
                (int)_renderSize.Y);
        }

        internal virtual void UpdateInnerArea()
        {
            InnerArea = new Rectangle(
                (int)_innerRenderPosition.X, 
                (int)_innerRenderPosition.Y, 
                (int)_innerRenderSize.X, 
                (int)_innerRenderSize.Y
                );
        }

        internal virtual void UpdateColors()
        {
            if (IsEnabled)
            {
                if (IsPressed)
                {
                    _currentColor = _pressedColor;
                    _currentBorderColor = _pressedBorderColor;
                }
                else if (IsFocused)
                {
                    _currentColor = _focusedColor;
                    _currentBorderColor = _focusedBorderColor;
                }
                else if (IsHighlighted)
                {
                    _currentColor = _highlightedColor;
                    _currentBorderColor = _highlightedBorderColor;
                }
                else
                {
                    _currentColor = _backColor;
                    _currentBorderColor = _borderColor;
                }
            }
            else
            {
                _currentColor = _disabledColor;
                _currentBorderColor = _disabledBorderColor;
            }
        }

        internal virtual void UpdateCullRectangle()
        {
            _cullRectangle = RenderArea;
            if (Parent != null)
                _cullRectangle = Rectangle.Intersect(_cullRectangle, Parent._cullRectangle);
        }

        protected override void BeginDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            G.GraphicsDevice.ScissorRectangle = _cullRectangle;
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (_currentBorderColor.A > 0 && BorderSize.X > 0 && BorderSize.Y > 0)
                spriteBatch.DrawRectangle(RenderArea, _currentBorderColor);
            if (_currentColor.A > 0)
                spriteBatch.DrawRectangle(InnerArea, _currentColor);
        }

#if WINDOWS
        protected internal virtual bool ProcessMouseMove()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseMove();
                if (handled)
                    break;
            }
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    MouseIsInside = true;
                    //if (mouse.ScrollWheelDelta != 0)
                    //    OnMouseScroll();
                    OnMouseMove();
                    handled = true;
                }
                else
                {
                    MouseIsInside = false;
                    handled = false;
                }
            }
            return handled;
        }

        internal virtual bool ProcessMouseDown()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseDown();
                if (handled)
                    break;
            }
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    if (!IsPressed && IsEnabled)
                    {
                        IsPressed = true;
                        OnMouseDown();
                        if (IsFocusable && !IsFocused)
                            IsFocused = true;
                    }
                    handled = true;
                }
                else
                    handled = false;
            }
            return handled;
        }

        internal virtual bool ProcessMouseUp()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseUp();
                if (handled)
                    break;
            }
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(G.Input.Mouse.Position))
                {
                    IsPressed = false;
                    OnMouseUp();
                    handled = true;
                }
                else
                    handled = false;
            }
            return handled;
        }

        protected virtual void OnMouseMove()
        {
        }

        protected virtual void OnMouseDown()
        {
        }

        protected virtual void OnMouseUp()
        {
        }

        protected virtual void OnPressed()
        {
            if (G.UI.PressedControl != this)
            {
                if (G.UI.PressedControl != null)
                    G.UI.PressedControl.OnReleased();
                G.UI.PressedControl = this;
            }
        }

        protected virtual void OnReleased()
        {
            if (G.UI.PressedControl == this)
                G.UI.PressedControl = null;
        }

        protected virtual void OnMouseEnter()
        {
            if (G.UI.EnteredControl != this)
            {
                if (G.UI.EnteredControl != null)
                    G.UI.EnteredControl.OnMouseLeave();
                G.UI.EnteredControl = this;
            }
            IsHighlighted = true;
            if (G.Input.Mouse.LeftButton.IsDown)
            {
                if (G.UI.PressedControl != this)
                    return;
                IsPressed = true;
            }
        }

        protected virtual void OnMouseLeave()
        {
            if (G.UI.EnteredControl == this)
                G.UI.EnteredControl = null;
            IsHighlighted = false;
            if (G.Input.Mouse.LeftButton.IsDown)
            {
                if (G.UI.PressedControl == this)
                {
                    IsHighlighted = false;
                    IsPressed = false;
                }
                else
                    return;
            }
        }

        protected virtual void OnMouseScroll()
        {
        }

        public virtual void ProcessKeyDown(Keys key)
        {
            OnKeyDown(key);
        }

        public virtual void ProcessKeyPress(Keys key)
        {
            OnKeyPress(key);
        }

        public virtual void ProcessKeyUp(Keys key)
        {
            OnKeyUp(key);
        }

        protected virtual void OnKeyDown(Keys key)
        {
        }

        protected virtual void OnKeyPress(Keys key)
        {
        }

        protected virtual void OnKeyUp(Keys key)
        {
        }

#endif
        protected virtual void OnGotFocus()
        {
            if (G.UI.FocusedControl != this)
            {
                if (G.UI.FocusedControl != null)
                    G.UI.FocusedControl.IsFocused = false;
                G.UI.FocusedControl = this;
            }
        }

        protected virtual void OnLostFocus()
        {
            IsPressed = false;
            G.UI.FocusedControl = null;
        }

        protected virtual void OnPositionChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual void OnSizeChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual void OnBorderSizeChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual bool IntersectTest(Point point)
        {
            return RenderArea.Intersects(new Rectangle(point.X, point.Y, 1, 1));
        }

        public void Focus()
        {
            IsFocused = true;
        }

        public void Invalidate()
        {
            NeedsHeavyUpdate = true;
        }

        public override string ToString()
        {
            return String.Format("Name: {0}, Position: ({1},{2}), Size: ({3},{4})", Name, _renderPosition.X, _renderPosition.Y, _renderSize.X, _renderSize.Y);
        }

    }
}
