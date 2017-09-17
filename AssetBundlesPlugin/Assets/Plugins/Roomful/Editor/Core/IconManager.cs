using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public static class IconManager   {

        private static Dictionary<Icon, Texture2D> s_icons = new Dictionary<Icon, Texture2D>();
        private static Dictionary<float, Texture2D> s_colorIcons = new Dictionary<float, Texture2D>();


        public static Texture2D GetIcon(Color color) {
            float colorId = color.r * 1000f + color.g * 100f + color.b * 10f + color.a;

            if (s_colorIcons.ContainsKey(colorId)) {
                return s_colorIcons[colorId];
            } else {


                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, color);
                tex.Apply();
                

                s_colorIcons.Add(colorId, tex);
                return GetIcon(color);
            }
        }

        public static Texture2D GetIcon(Icon icon) {

            if(s_icons.ContainsKey(icon)) {
                return s_icons[icon];
            } else {
                Texture2D tex = Resources.Load(icon.ToString()) as Texture2D;
                if(tex == null) {
                    tex = new Texture2D(1, 1);
                }

                s_icons.Add(icon, tex);
                return GetIcon(icon);
            }
        }


        public static Texture2D Rotate(Texture2D tex, float angle) {
            Texture2D rotImage = new Texture2D(tex.width, tex.height);
            int x, y;
            float x1, y1, x2, y2;

            int w = tex.width;
            int h = tex.height;
            float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
            float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

            float dx_x = rot_x(angle, 1.0f, 0.0f);
            float dx_y = rot_y(angle, 1.0f, 0.0f);
            float dy_x = rot_x(angle, 0.0f, 1.0f);
            float dy_y = rot_y(angle, 0.0f, 1.0f);



            x1 = x0;
            y1 = y0;

            for (x = 0; x < tex.width; x++) {
                x2 = x1;
                y2 = y1;
                for (y = 0; y < tex.height; y++) {
                    //rotImage.SetPixel (x1, y1, Color.clear);          

                    x2 += dx_x;//rot_x(angle, x1, y1);
                    y2 += dx_y;//rot_y(angle, x1, y1);
                    rotImage.SetPixel((int)Mathf.Floor(x), (int)Mathf.Floor(y), getPixel(tex, x2, y2));
                }

                x1 += dy_x;
                y1 += dy_y;

            }

            rotImage.Apply();
            return rotImage;
        }


        private static Color getPixel(Texture2D tex, float x, float y) {
            Color pix;
            int x1 = (int)Mathf.Floor(x);
            int y1 = (int)Mathf.Floor(y);

            if (x1 > tex.width || x1 < 0 ||
               y1 > tex.height || y1 < 0) {
                pix = Color.clear;
            } else {
                pix = tex.GetPixel(x1, y1);
            }

            return pix;
        }

        private static float rot_x(float angle, float x, float y) {
            float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
            float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
            return (x * cos + y * (-sin));
        }
        private static float rot_y(float angle, float x, float y) {
            float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
            float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
            return (x * sin + y * cos);
        }




    }
}
