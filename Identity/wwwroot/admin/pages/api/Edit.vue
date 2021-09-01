<template>
  <div>
    <div v-if="!apiResource" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <form v-if="apiResource" @submit.prevent="save()">
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h3>{{$localizer('API')}}</h3>
        <div class="btn-toolbar mb-2 mb-md-0">
          <button type="submit" class="btn btn-sm btn-outline-secondary">{{$localizer('Save')}}</button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody>
            <tr>
              <th class="w-25">{{$localizer('Name')}}</th>
              <td><input type="text" v-model="apiResource.name" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('DisplayName')}}</th>
              <td><input type="text" v-model="apiResource.displayName" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('Secret')}}</th>
              <td><input type="text" v-model="apiResource.secret" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('Enabled')}}</th>
              <td><i class="h4" :class="{'icon-toggle-on text-success': apiResource.enabled, 'icon-toggle-off text-muted': !apiResource.enabled}" @click="apiResource.enabled = !apiResource.enabled"></i></td>
            </tr>
          </tbody>
        </table>
      </div>
    </form>
  </div>
</template>

<script>
module.exports = {
  data: function () {
    return {
      apiResource: null
    };
  },
  methods: {
    save() {
      api.saveApiResource(this.apiResource).then(response => {
        this.$notyf.success(this.$localizer('Saved'));
      }).catch(error => {
        this.$notyf.error(this.$localizer('Error'));
      });
    }
  },
  created() {
    if (this.$route.params.id === 'new') {
      this.apiResource = {
        name: '',
        displayName: '',
        secret: '',
        enabled: true
      };
    } else {
      api.getApiResource(this.$route.params.id).then(apiResource => this.apiResource = apiResource);
    }
  }
}
</script>
