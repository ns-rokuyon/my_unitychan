using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public static class ColoExtension {
        public static Color add(this Color col, float diff_r, float diff_g, float diff_b, float diff_a) {
            return new Color(col.r + diff_r,
                             col.g + diff_g,
                             col.b + diff_b,
                             col.a + diff_a);
        }

        public static Color sub(this Color col, float diff_r, float diff_g, float diff_b, float diff_a) {
            return new Color(col.r - diff_r,
                             col.g - diff_g,
                             col.b - diff_b,
                             col.a - diff_a);
        }

        public static Color mul(this Color col, float diff_r, float diff_g, float diff_b, float diff_a) {
            return new Color(col.r * diff_r,
                             col.g * diff_g,
                             col.b * diff_b,
                             col.a * diff_a);
        }

        public static Color div(this Color col, float diff_r, float diff_g, float diff_b, float diff_a) {
            return new Color(col.r / diff_r,
                             col.g / diff_g,
                             col.b / diff_b,
                             col.a / diff_a);
        }

        public static Color changeR(this Color col, float new_r) {
            return new Color(new_r, col.g, col.b, col.a);
        }

        public static Color changeG(this Color col, float new_g) {
            return new Color(col.r, new_g, col.b, col.a);
        }

        public static Color changeB(this Color col, float new_b) {
            return new Color(col.r, col.g, new_b, col.a);
        }

        public static Color changeA(this Color col, float new_a) {
            return new Color(col.r, col.g, col.b, new_a);
        }


        public static Color32 add(this Color32 col, byte diff_r, byte diff_g, byte diff_b, byte diff_a) {
            return new Color32((byte)(col.r + diff_r),
                               (byte)(col.g + diff_g),
                               (byte)(col.b + diff_b),
                               (byte)(col.a + diff_a));
        }

        public static Color32 sub(this Color32 col, byte diff_r, byte diff_g, byte diff_b, byte diff_a) {
            return new Color32((byte)(col.r - diff_r),
                               (byte)(col.g - diff_g),
                               (byte)(col.b - diff_b),
                               (byte)(col.a - diff_a));
        }

        public static Color32 changeR(this Color32 col, byte new_r) {
            return new Color32(new_r, col.g, col.b, col.a);
        }

        public static Color32 changeG(this Color32 col, byte new_g) {
            return new Color32(col.r, new_g, col.b, col.a);
        }

        public static Color32 changeB(this Color32 col, byte new_b) {
            return new Color32(col.r, col.g, new_b, col.a);
        }

        public static Color32 changeA(this Color32 col, byte new_a) {
            return new Color32(col.r, col.g, col.b, new_a);
        }
    }
}