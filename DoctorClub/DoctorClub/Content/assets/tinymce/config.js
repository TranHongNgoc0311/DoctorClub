tinymce.init({
  selector: "textarea#content",
    theme: 'silver',
    mobile: {
        theme: 'silver',
        height: 450,
        plugins: ['autosave', 'lists', 'autolink'],
        toolbar: ['bold', 'italic']
    },
  width: "",
  height: 300,
  plugins: [
  "advlist autolink link image lists charmap print preview hr anchor pagebreak",
  "fullscreen save template textpattern imagetools",
  "searchreplace wordcount visualblocks visualchars insertdatetime media nonbreaking",
  "table directionality emoticons paste code responsivefilemanager"
  ],
  toolbar1: "undo redo | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | styleselect",
  toolbar2: "| insert | wordcount | responsivefilemanager | link unlink anchor | image media | forecolor backcolor  | print preview code ",
  image_advtab: true ,
  code_dialog_height: 200,
  encoding: "xml",
  statusbar: false,
  branding: false,
  entity_encodig: "raw",
  schema: "html5",
  toolbar_items_size: "small",
  element_format: "html",
  force_br_newlines: true,
  force_p_newlines: false
});

tinymce.init({
    selector: "textarea.edit-content",
    theme: 'silver',
    mobile: {
        theme: 'silver',
        height: 450,
        plugins: ['autosave', 'lists', 'autolink'],
        toolbar: ['bold', 'italic']
    },
    width: "",
    height: 300,
    plugins: [
        "advlist autolink link image lists charmap print preview hr anchor pagebreak",
        "fullscreen save template textpattern imagetools",
        "searchreplace wordcount visualblocks visualchars insertdatetime media nonbreaking",
        "table directionality emoticons paste code responsivefilemanager"
    ],
    toolbar1: "undo redo | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | styleselect",
    toolbar2: "| insert | wordcount | responsivefilemanager | link unlink anchor | image media | forecolor backcolor  | print preview code ",
    image_advtab: true,
    code_dialog_height: 200,
    encoding: "xml",
    statusbar: false,
    branding: false,
    entity_encodig: "raw",
    schema: "html5",
    toolbar_items_size: "small",
    element_format: "html",
    force_br_newlines: true,
    force_p_newlines: false
});
