tinymce.init({
  selector: "textarea#content",
    theme: 'silver',
    mobile: {
        theme: 'silver',
        height: 450,
        plugins: ['autosave', 'lists', 'autolink'],
        toolbar: ['bold', 'italic']
    },
  height: 380,
  plugins: [
  "autolink link image lists charmap print preview hr anchor pagebreak",
  "fullscreen save template textpattern placeholder",
  "searchreplace wordcount visualblocks visualchars insertdatetime nonbreaking",
  "table directionality emoticons paste code"
  ],
  toolbar1: "undo redo | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | styleselect",
  toolbar2: "| insert | wordcount | responsivefilemanager | link unlink anchor | forecolor backcolor  | print preview code ",
  relative_urls: false,
  convert_urls: false,
  remove_script_host: false,
  code_dialog_height: 200,
  encoding: "html",
  statusbar: false,
  branding: false,
  entity_encodig: "raw",
  schema: "html5",
  toolbar_items_size: "small",
  element_format: "html",
  force_br_newlines: true,
  force_p_newlines: false,
  forced_root_block: ""
});
