export interface ICreateContainerRequest {
  name: string
  description: string
  projectId: string
  parentContainerId?: string
}
