import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', () => {
  const psgCoutNum = ref(27)
  const psgTagNum = ref(5)
  const psgSortNum = ref(21)
  return { psgCoutNum, psgTagNum, psgSortNum }
})
