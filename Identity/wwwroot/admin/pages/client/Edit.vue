<template>
  <div class="pb-5">
    <div v-if="!client" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <form v-if="client" @submit.prevent="save()">
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h3>{{$localizer('Client')}}</h3>
        <div class="btn-toolbar mb-2 mb-md-0">
          <button type="submit" class="btn btn-sm btn-outline-secondary">{{$localizer('Save')}}</button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody>
            <tr>
              <th class="w-25">{{$localizer('ClientName')}}</th>
              <td><input type="text" v-model="client.clientName" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('ClientId')}}</th>
              <td><input type="text" :value="client.clientId" :placeholder="$localizer('Save to generate...')" class="form-control form-control-sm" disabled /></td>
            </tr>
            <tr>
              <th>{{$localizer('ClientSecret')}}</th>
              <td>
                <div class="input-group">
                  <input :type="secretType" :value="client.clientSecret" :placeholder="$localizer('Save to generate...')" class="form-control form-control-sm" disabled />
                  <div class="input-group-append">
                    <button type="button" class="btn btn-sm btn-outline-secondary" @click="secretHidden = !secretHidden"><i :class="{'icon-eye-off': secretHidden, 'icon-eye': !secretHidden}"></i></button>
                  </div>
                </div>
              </td>
            </tr>
            <tr>
              <th>{{$localizer('Enabled')}}</th>
              <td><i class="h4" :class="{'icon-toggle-on text-success': client.enabled, 'icon-toggle-off text-muted': !client.enabled}" @click="client.enabled = !client.enabled"></i></td>
            </tr>
            <tr>
              <th>{{$localizer('Logo')}}</th>
              <td>
                <div class="logo-preview border rounded" :class="{'no-img': !client.logoUri}" :style="{backgroundImage: `url(${client.logoUri})`}">
                  <input v-if="!client.logoUri" type="file" accept="image/*" @change="onChangeLogo" />
                  <div class="overlay" :class="{'icon-x': client.logoUri,'icon-upload-cloud': !client.logoUri}" @click="clearLogo()"></div>
                </div>
                <!-- <div class="custom-file">
                  <input type="file" class="custom-file-input" id="logoFile" @change="onChangeLogo" />
                  <label class="custom-file-label" for="logoFile">{{$localizer('Choose file')}}</label>
                </div> -->
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- AllowedUris -->
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center">
        <h5 class="mt-3">{{$localizer('AllowedUris')}}</h5>
        <div class="btn-toolbar mb-2 mb-md-0">
          <button type="button" class="btn btn-sm btn-outline-secondary" @click="client.allowedUris.push('')">
            <i class="icon-plus-circle"></i>
            {{$localizer('Add')}}
          </button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <thead>
            <tr>
              <th>{{$localizer('URL')}}</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="!client.allowedUris.length">
              <td colspan="2" class="font-weight-bold text-muted font-italic py-2">{{$localizer('No allowed uris')}}</td>
            </tr>
            <tr v-for="(allowedUri, index) in client.allowedUris">
              <td class="align-middle">
                <input type="text" v-model="client.allowedUris[index]" class="form-control form-control-sm" placeholder="https://localhost" />
              </td>
              <td class="align-middle text-right">
                <button type="button" class="btn btn-sm btn-outline-secondary" @click="client.allowedUris.splice(index, 1)"><i class="icon-trash-2"></i></button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- =========== -->

      <!-- Danger Zone -->
      <div v-if="!isNew" class="list-group alert-danger mt-3">
        <div class="list-group-item border-danger p-0" style="overflow: hidden;">
          <h5 class="alert-danger p-2 m-0">{{$localizer('Danger Zone')}}</h5>
        </div>
        <div class="list-group-item d-flex justify-content-between align-items-start border-danger">
          <div>
            <h6 class="mb-1">{{$localizer('Delete this client')}}</h6>
            <p class="m-0">{{$localizer('NoGoingBackDelete')}}</p>
          </div>
          <button type="button" class="btn btn-outline-danger" @click="deleteClient()">{{$localizer('Delete')}}</button>
        </div>
      </div>
      <!-- =========== -->
    </form>
  </div>
</template>

<script>
module.exports = {
  data: function () {
    return {
      client: null,
      secretHidden: true
    };
  },
  computed: {
    isNew() {
      return this.$route.params.id === 'new';
    },
    secretType() {
      return this.secretHidden ? 'password' : 'text';
    }
  },
  methods: {
    save() {
      api.saveClient(this.client).then(response => {
        this.$notyf.success(this.$localizer('Saved'));
        this.$router.push(`/client/${response.clientId}`);
      }).catch(error => {
        this.$notyf.error(this.$localizer('Error'));
      });
    },
    deleteClient() {
      swal({
        title: this.$localizer("Delete?"),
        text: this.$localizer("This action can't be undone."),
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then((willDelete) => {
        if (willDelete) {
          api.deleteClient(this.client.clientId).then(() => {
            notyf.success(this.$localizer('Client removed'));
            this.$router.push('/client');
          }).catch(error => {
            this.$notyf.error(this.$localizer('Error'));
          });
        }
      });
    },
    onChangeLogo(event) {
      var files = event.target.files || event.dataTransfer.files;
      if(!files.length) return;
      const reader = new FileReader();
    	reader.onload = (event) => {
    		this.client.logoUri = event.target.result;
    	};
    	reader.readAsDataURL(files[0]);
    },
    clearLogo() {
      if(this.client.logoUri) this.client.logoUri = '';
    },
    loadData() {
      this.client = null;
      if (this.isNew) {
        this.client = {
          clientName: '',
          logoUri: '',
          enabled: true,
          allowedUris: []
        };
      } else {
        api.getClient(this.$route.params.id).then(client => this.client = client);
      }
    }
  },
  watch: {
    "$route.params.id": function () {
      this.loadData();
    }
  },
  created() {
    this.loadData();
  }
}
</script>

<style lang="css" scoped>
.logo-preview {
  position: relative;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  width: 142px;
  height: 80px;
  background-color: rgba(0, 0, 0, 0.15);
  overflow: hidden;
}
.logo-preview input[type="file"] {
  position: absolute;
	z-index: 2;
	top: 0;
	right: 0;
	width: 200%;
	height: 100%;
	opacity: 0;
	background-color: transparent;
	color: transparent;
  cursor: pointer;
}
.logo-preview .overlay {
  position: absolute;
	top: 0;
	left: 0;
	width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #000;
	color: #fff;
  font-size: 30px;
  cursor: pointer;
  opacity: 0;
  transition: opacity 0.3s ease;
}
.logo-preview.no-img .overlay {
	opacity: 0.5;
}
.logo-preview:hover .overlay {
	opacity: 0.7;
}
</style>
