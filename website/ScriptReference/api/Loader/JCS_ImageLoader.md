# JCS_ImageLoader

Image loader, load an external image file to Unity usable sprite.


## Functions

<table>
  <tr>
    <td>LoadTexture</td>
    <td>Load image file as texture.</td>
  </tr>
  <tr>
    <td>Create</td>
    <td>Create sprite object.</td>
  </tr>
  <tr>
    <td>LoadImage</td>
    <td>Load Image by file path.</td>
  </tr>
</table>


## Example

```cs
string filePath = "path/to/image.png";

// Get the image as texture.
Texture2D tex = JCS_ImageLoader.LoadTexture(filePath);

// Convert texture to sprite.
Sprite sp = JCS_ImageLoader.Create(tex);

// Combine the two functions just use.
sp = JCS_ImageLoader.LoadImage(filePath);
```