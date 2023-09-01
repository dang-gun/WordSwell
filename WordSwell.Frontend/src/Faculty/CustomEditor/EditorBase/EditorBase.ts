import { Alignment } from '@ckeditor/ckeditor5-alignment';
import { Bold, Code, Italic, Strikethrough, Underline, Superscript, Subscript } from '@ckeditor/ckeditor5-basic-styles';
import { BlockQuote } from '@ckeditor/ckeditor5-block-quote';
import { CodeBlock } from '@ckeditor/ckeditor5-code-block';
import ClassicEditorBase from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';
import { Essentials } from '@ckeditor/ckeditor5-essentials';
import { Heading } from '@ckeditor/ckeditor5-heading';
import { Indent } from '@ckeditor/ckeditor5-indent';
import { Link, LinkImage } from '@ckeditor/ckeditor5-link';
import { List, TodoList } from '@ckeditor/ckeditor5-list';
import { MediaEmbed } from '@ckeditor/ckeditor5-media-embed';
import { Paragraph } from '@ckeditor/ckeditor5-paragraph';
import { Table } from '@ckeditor/ckeditor5-table';
import { Base64UploadAdapter, FileRepository } from '@ckeditor/ckeditor5-upload';
import { AutoImage, Image, ImageCaption, ImageInsert, ImageResize, ImageStyle, ImageToolbar, ImageUpload, ImageUploadEditing } from '@ckeditor/ckeditor5-image';
import { Markdown } from '@ckeditor/ckeditor5-markdown-gfm';
import { Autoformat } from '@ckeditor/ckeditor5-autoformat';
import GlobalStatic from '@/Global/GlobalStatic';
import { GeneralHtmlSupport } from '@ckeditor/ckeditor5-html-support';
import { HorizontalLine } from '@ckeditor/ckeditor5-horizontal-line';

export default class EditorBase extends ClassicEditorBase { }

EditorBase.builtinPlugins = [
    Essentials,
    Bold,
    Italic,
    Superscript,
    Subscript,
    Underline,
    Strikethrough,
    Paragraph,
    Alignment,
    CodeBlock,
    BlockQuote,
    Heading,
    Link,
    List,
    Code,
    MediaEmbed,
    Table,
    Indent,
    // Base64UploadAdapter,
    // SimpleUploadAdapter,
    FileRepository,
    Image,
    ImageToolbar,
    ImageCaption,
    ImageStyle,
    ImageResize,
    ImageInsert,
    AutoImage,
    LinkImage,
    ImageUpload,
    ImageUploadEditing,
    Autoformat,
    GeneralHtmlSupport,
    HorizontalLine,
    TodoList,
    Markdown
];

EditorBase.defaultConfig = {
    toolbar: {
        items: [
            'undo', 'redo',
            '|', 'heading',
            '|', 'bold', 'italic', 'underline', 'strikethrough',
            '|', 'bulletedList', 'numberedList',
            '|', 'alignment', 'outdent', 'indent',
            '|', 'insertTable', 'mediaEmbed',
            '|', 'link', 'blockQuote', 'codeBlock', 'code'
        ]
    },
    image: {
        toolbar: [
            'imageStyle:inline',
            'imageStyle:block',
            'imageStyle:side',
            '|',
            'toggleImageCaption',
            'imageTextAlternative'
        ],
        upload: {
            types: ['png', 'jpeg', 'jpe', 'jpg', 'gif', 'bmp', 'webp', 'wbmp', 'ico', 'jfif', 'jif', 'tif', 'tiff', 'pbg'],
        }
    },
    // simpleUpload: {
    //     uploadUrl: 'http:localhost:3000/api/upload/image',
    //     withCredentials: true,
    // },
    language: 'ko',
    placeholder: GlobalStatic.EditorPlaceholder,
};

export function toggleMarkdownPlugin()
{
    if (GlobalStatic.EditorMode === 'wysiwyg')
    {
        console.log('마크다운 추가함');
        EditorBase.builtinPlugins.push(Markdown);
    }
    else
    {
        console.log('마크다운 제거함');
        EditorBase.builtinPlugins.pop();
    }
}