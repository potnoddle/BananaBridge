import os
import json
import glob

def trim_styles():
    source_dir = r"C:\Work\Projects\StoryGenerator\configs\styles"
    dest_file = r"C:\Work\Projects\Paint.Net-Plugings\AI Editor\styles.json"
    
    trimmed_list = []
    
    # Search for all json files in the directory
    json_files = glob.glob(os.path.join(source_dir, "*.json"))
    for file_path in json_files:
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                data = json.load(f)
                styles = data.get("styles", [])
                for style in styles:
                    trimmed_style = {
                        "id": style.get("id", ""),
                        "name": style.get("name", ""),
                        "description": style.get("description", ""),
                        "image_prompt": style.get("image_prompt", "")
                    }
                    # Only add if it has a name
                    if trimmed_style["name"]:
                        trimmed_list.append(trimmed_style)
        except Exception as e:
            print(f"Error parsing {file_path}: {e}")
            
    # Write to destination file
    with open(dest_file, 'w', encoding='utf-8') as f:
        json.dump(trimmed_list, f, indent=2, ensure_ascii=False)
        
    print(f"Successfully trimmed and combined {len(trimmed_list)} styles to {dest_file}")

if __name__ == "__main__":
    trim_styles()
