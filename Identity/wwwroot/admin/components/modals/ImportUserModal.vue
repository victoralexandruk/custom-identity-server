<template lang="html">
  <div class="modal fade" id="modalImportUsers" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <form @submit.prevent="save()">
          <div class="modal-header">
            <h5 class="modal-title">{{$localizer('Import')}}</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <div v-if="!saving">
              <div class="input-group mb-3">
                <div class="input-group-prepend">
                  <button type="button" class="btn btn-sm btn-secondary" @click="downloadTemplate()">
                    <i class="icon-download"></i>
                    {{$localizer('Template')}}
                  </button>
                </div>
                <div class="custom-file">
                  <input type="file" class="custom-file-input" id="uploadFile" @change="onChangeFile" required />
                  <label class="custom-file-label" for="uploadFile">{{$localizer('Choose file')}}</label>
                </div>
              </div>
              <div v-if="fileData" class="table-responsive table-overflow">
                <table class="table table-striped table-sm">
                  <thead class="thead-light">
                    <tr>
                      <th>#</th>
                      <th>{{$localizer('UserName')}}</th>
                      <th>{{$localizer('FullName')}}</th>
                      <th>{{$localizer('Email')}}</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-if="!fileData.length">
                      <td colspan="4" class="font-weight-bold text-muted font-italic py-2">{{$localizer('No data')}}</td>
                    </tr>
                    <tr v-for="(row, index) in fileData">
                      <th class="align-middle">{{index + 1}}</th>
                      <td class="align-middle"><input type="text" v-model="row.userName" class="form-control form-control-sm" required /></td>
                      <td class="align-middle"><input type="text" v-model="row.fullName" class="form-control form-control-sm" required /></td>
                      <td class="align-middle"><input type="text" v-model="row.email" class="form-control form-control-sm" /></td>
                      <td class="align-middle text-right">
                        <button type="button" class="btn btn-sm btn-outline-secondary" @click="fileData.splice(index, 1)"><i class="icon-trash-2"></i></button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            <div v-if="saving" class="text-center">
              <h4>
                <span class="spinner-grow text-primary"></span>
                {{$localizer('Saving...')}}
              </h4>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal" :disabled="saving">{{$localizer('Cancel')}}</button>
            <button type="submit" class="btn btn-primary" :disabled="saving">{{$localizer('Confirm')}}</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
module.exports = {
  data() {
    return {
      fileData: null,
      saving: false
    };
  },
  methods: {
    downloadTemplate() {
      const workbook = XLSX.utils.book_new();
      const header = ['UserName','FullName','Email'];
      const data = [];
      const ws = XLSX.utils.aoa_to_sheet([header].concat(data));
      XLSX.utils.book_append_sheet(workbook, ws, 'Users');
      XLSX.writeFile(workbook, 'template_users.xlsx');
    },
    onChangeFile(event) {
      this.fileData = null;
      const files = event.target.files || event.dataTransfer.files;
      if (!files.length) return;
      const reader = new FileReader();
      reader.onload = (event) => {
  			const workbook = XLSX.read(event.target.result, {type: 'binary'});
  			const sheetName = workbook.SheetNames[0];
        const fileData = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
  			this.fileData = fileData.map(x => ({
          userName: x.UserName,
          fullName: x.FullName,
          email: x.Email,
          active: true
        }));
  		};
  		reader.readAsBinaryString(files[0]);
    },
    save() {
      try {
        if (!this.fileData.length || this.saving) return;
        this.saving = true;
        const savingNotification = this.$notyf.open({
          className: 'bg-info',
          icon: {
            className: 'spinner-grow spinner-grow-sm text-white align-baseline',
            tagName: 'i'
          },
          message: this.$localizer('Saving users...'),
          dismissible: false,
          duration: 0
        });
        api.saveUsers(this.fileData).then(response => {
          this.$notyf.dismiss(savingNotification);
          this.$notyf.success(this.$localizer('Saved'));
          this.$emit('save');
        }).catch(error => {
          this.saving = false;
          this.$notyf.error(this.$localizer('Error'));
        });
        $('#modalImportUsers').modal('hide');
        this.fileData = null;
        this.saving = false;
      } catch (e) {
        this.saving = false;
        this.$notyf.error(this.$localizer('Error'));
      }
    }
  }
}
</script>

<style lang="css" scoped>
td input {
  background: none;
}
.table-overflow {
  max-height: 300px;
  overflow: auto;
}
.table-overflow thead {
  position: sticky;
  top: 0;
  z-index: 1;
}
</style>
