import { FileLoader } from '@ckeditor/ckeditor5-upload';
import { ClassicEditor } from "@ckeditor/ckeditor5-editor-classic";
import GlobalStatic from '@/Global/GlobalStatic';

export class CustomUploadAdapter
{
    constructor(private loader: any)
    {
        this.loader = loader;
    }

    public upload()
    {
        return this.loader.file
            .then((file: File) =>
            {
                return new Promise((resolve, reject) =>
                {
                    const reader = new FileReader();
                    reader.onload = () =>
                    {
                        resolve({ default: reader.result, });
                    };
                    reader.readAsDataURL(file);

                    GlobalStatic.FileSelector.AddFile([file]);
                    console.log(GlobalStatic.FileSelector);
                });
            });
    }
}

export function CustomUploadAdapterPlugin(editor: ClassicEditor)
{
    editor.plugins.get('FileRepository').createUploadAdapter = (loader: FileLoader) =>
    {
        return new CustomUploadAdapter(loader);
    };
}