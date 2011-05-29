require 'rocco/tasks'

desc "Build Rocco Docs"
Rocco::make '../baler-docs/', 'CodeSlice.Web.Baler/CodeSlice.Web.Baler/*.cs', {
  :language => 'csharp'
}
Rocco::make '../baler-docs/', 'CodeSlice.Web.Baler/CodeSlice.Web.Baler.Extensions.*/*.cs', {
  :language => 'csharp'
}