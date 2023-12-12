export const tourData = [
  {
    "name": "The Workflow section",
    "alias": "workflowSection",
    "hidden": true,
    "requiredSections": [
      "content",
      "workflow"
    ],
    "steps": [
      {
        "title": "Workflow section",
        "content": "<p>Workflow adds a new section to the Backoffice. Click the Workflow link to get started</p>",
        "element": "#applications",
        "event": "click",
        "eventElement": "[data-element='section-workflow']",
        "backdropOpacity": 0.6,
        "skipStepIfVisible": "[data-element='tree-item-active']"
      },
      {
        "title": "Workflow section",
        "content": "<p>Let's explore the Workflow section. Try clicking the 'Active workflows' menu item at the top of the tree</p>",
        "element": "[data-element='tree-item-active']",
        "eventElement": "[data-element='tree-item-active'] a",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Active workflows",
        "content": "<p>Once your users start submitting content for approval, Workflow administrators will be able to see all active workflows in this view. Most likely there's nothing here, yet.</p>",
        "element": "#contentcolumn",
        "elementPreventClick": true
      },
      {
        "title": "Appproval Groups",
        "content": "<p>Next, let's get to know Workflow's approval groups. Click the menu item to continue.</p>",
        "element": "[data-element='tree-item-approval-groups']",
        "event": "click",
        "eventElement": "[data-element='tree-item-approval-groups'] a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Appproval Groups",
        "content": "<p>Approval groups are a core feature of Workflow - you'll use these groups to build approval flows for your content.</p><p>The data generator has created a set of groups and added users to each.</p>",
        "element": "#contentcolumn"
      },
      {
        "title": "Appproval Groups",
        "content": "<p>Clicking the group name loads the group details.</p>",
        "element": "[data-element='editor-container'] .umb-table-body .umb-table-row:first-child",
        "eventElement": "[data-element='editor-container'] .umb-table-body .umb-table-row:first-child .umb-table-body__link",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Appproval Groups - Settings",
        "content": "<p>The first view displays the generic settings for the group, plus the activity chart.</p>",
        "element": "[data-element='editor-container']"
      },
      {
        "title": "Appproval Groups - Roles",
        "content": "<p>The Roles app displays the current permissions for the group. Click the button to load the Roles view.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-roles']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Appproval Groups - Roles",
        "content": "<p>The Roles view lists the current permissions for the approval group, for both document and document-type approvals (license required).</p><p>Permissions can not be edited from this view, but using the edit link on the right side of the roles table will load the corresponding document for updating.</p>",
        "element": "[data-element='editor-container']"
      },
      {
        "title": "Appproval Groups - Members",
        "content": "<p>The Members app displays the current membership for the group. Click the button to load the Members view.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-members']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Appproval Groups - Members",
        "content": "<p>The Members view lists the current membership for the approval group, for both explicit and inherited membership.</p><ul><li><strong>Explicit members:</strong> are set directly on this group. Users can be members of one or many groups.</li><li><strong>Inherited members:</strong> are inherited from existing Umbraco user groups. This is useful for sites with a large number of Umbraco user groups, as it can reduce duplication between Workflow and Umbraco groups.</li></ul><p>A group can contain both explicit and inherited members, and will be automatically de-duplicated.</p>",
        "element": "[data-element='editor-container']"
      },
      {
        "title": "Appproval Groups - History",
        "content": "<p>The History app displays the workflow history for the group. Click the button to load the History view.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-history']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Appproval Groups - History",
        "content": "<p>There's probably nothing here, but there will be in the future once your editors start approving workflow processes.</p>",
        "element": "[data-element='editor-container']"
      },
      // {
      //   "title": "Appproval Groups - History",
      //   "content": "<p>All history views can be filtered to refine the result set. Click the filter button to open the overlay.</p>",
      //   "element": "[data-element='editor-container']",
      //   "eventElement": "[data-element='editor-container'] .umb-box-header workflow-filter-picker",
      //   "event": "click",
      //   "backdropOpacity": 0.6
      // },
      // {
      //   "title": "Appproval Groups - History",
      //   "content": "<p>Let's close the filters and move on to Content Reviews.</p>",
      //   "element": "[ng-controller^='Workflow.FilterPicker.Controller']",
      //   "eventElement": "[ng-controller^='Workflow.FilterPicker.Controller'] [label-key='general_close']",
      //   "event": "click",
      //   "backdropOpacity": 0.6
      // },
      {
        "title": "Content Reviews",
        "content": "<p>Content Reviews extend Workflow beyond content approvals. Click the menu item to continue.</p>",
        "element": "[data-element='tree-item-content-reviews']",
        "event": "click",
        "eventElement": "[data-element='tree-item-content-reviews'] a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Content Reviews - Overview",
        "content": "<p>The Overview will make more sense after exploring the settings, let's go there first.</p>",
        "element": "#contentcolumn",
        "eventElement": "[data-element='sub-view-settings']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Content Reviews - Settings",
        "content": "<p>Content Reviews allows configuring review periods for all content on your site. When the review period ends, your editors will be notified and requested to review the relevant content. Review periods can vary by document or document type, allowing granular control over who is required to review which pages, and how frequently those reviews take place.</p>",
        "element": "[data-element='editor-container']"
      },
      {
        "title": "Content Reviews - Overview",
        "content": "<p>Head back to the Overview by clicking the content app link.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-overview']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Content Reviews - Overview",
        "content": "<p>Like the history views, the Content Reviews overview is likely empty too - there's no content in the site requiring review. When there is, those documents will be listed here, and the responsible editors emailed.</p>",
        "element": "[data-element='editor-container']"
      },
      {
        "title": "Settings",
        "content": "<p>There's a lot to see in the Settings section, but we won't go into detail on everything. Click the tree item to continue.</p>",
        "element": "[data-element='tree-item-settings']",
        "event": "click",
        "eventElement": "[data-element='tree-item-settings'] a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Settings",
        "content": "<p>The Settings section is where you'll configure Workflow to work exactly as you need. All the options have descriptions explaining the impact of switching the toggles (and the documentation is super useful too). Some settings require a license, but all can be tested by enabling the test license in AppSettings by setting Umbraco:Workflow:EnableTestLicense to true.</p>",
        "element": "[data-element='editor-container']"
      }
    ]
  },
  {
    "name": "Workflow configuration",
    "alias": "workflowConfiguration",
    "hidden": true,
    "requiredSections": [
      "content",
      "workflow",
    ],
    "steps": [
      {
        "title": "Workflow configuration",
        "content": "<p>Let's return to the Content section and explore Workflow's configuration options.</p>",
        "element": "#applications",
        "event": "click",
        "eventElement": "[data-element='section-content']",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow Dashboard",
        "skipStepIfVisible": "[data-element='editor-content']",
        "content": "<p>The Workflow dashboard displays lists of all processes relevant to the current user.</p><ul><li><strong>Tasks requiring my approval:</strong> these tasks have been submitted by other users, and have reached a workflow stage where the current user is a member of the assigned group.</li><li><strong>My submissions:</strong> these tasks are workflows initiated by the current user.</li><li><strong>Stale content:</strong> these items are documents marked as requiring review by the current user, based on the configuration in the Content Reviews settings.</li></ul>",
        "element": "[data-element='dashboard']",
        "elementPreventClick": true
      },
      {
        "title": "Workflow configuration",
        "skipStepIfVisible": "[data-element='editor-content']",
        "content": "<p>Let's configure a workflow process, by visting a content node.</p>",
        "element": ".umb-tree ul",
        "event": "click",
        "eventElement": ".umb-tree ul .umb-tree-item:first-child a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Jump over the Workflow content app.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-workflow']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>And then click the Workflow configuration button.</p>",
        "element": "#workflowAppHeader",
        "eventElement": "#workflowAppHeader [label-key='workflow_configuration']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>The active workflow configuration is highlighted in green. This view also shows the other available approval flows for the current node:</p><ul><li><strong>Inherited approval flow:</strong> used when no flow set on the node, and no flow set for the document type (license required). When active, this flow checks up the tree until it finds a node with workflow configuration.</li><li><strong>Document Type approval flow:</strong> when no flow is set on the node, and the install is licensed, Workflow checks for Document Type configuration before falling back to inherited approvals.</li></ul>",
        "element": ".sub-view-Workflow",
        "elementPreventClick": true
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Try adding a group to the content approval workflow by clicking the Add button</p>",
        "element": "workflow-config .umb-box:first-of-type",
        "eventElement": "workflow-config .umb-node-preview-add",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Select a group</p>",
        "element": ".umb-user-group-picker-list",
        "eventElement": ".umb-user-group-picker-list-item:has(.umb-user-group-picker-list-item__icon) .umb-user-group-picker__action",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Submit the modal</p>",
        "element": ".umb-editors .umb-editor-footer",
        "eventElement": "[label-key='general_submit'] .umb-button",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Finally, save the updated configuration. When thsis node is sent for publishing approval, these groups will be required to approve the changes before the updated content can be published</p>",
        "element": "workflow-config .umb-box:first-of-type",
        "eventElement": "button:has([key='buttons_save']",
        "event": "click",
        "backdropOpacity": 0.6
      }
    ]
  },
  {
    "name": "Sending for approval",
    "alias": "workflowSendingForApproval",
    "hidden": true,
    "requiredSections": [
      "content",
      "workflow",
    ],
    "steps": [
      {
        "title": "Sending content for approval",
        "content": "<p>Let's return to the Content section and send content for workflow approval.</p>",
        "element": "#applications",
        "event": "click",
        "eventElement": "[data-element='section-content']",
        "backdropOpacity": 0.6
      },
      {
        "title": "Sending content for approval",
        "skipStepIfVisible": "[data-element='editor-content']",
        "content": "<p>Let's visit a content node.</p>",
        "element": ".umb-tree ul",
        "event": "click",
        "eventElement": ".umb-tree ul .umb-tree-item:first-child a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Workflow configuration",
        "content": "<p>Jump over the Workflow content app.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-workflow']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Sending content for approval",
        "content": "<p>Add a comment describing your hypothetical changes, then click the request publish button.</p>",
        "element": "workflow-submit",
        "eventElement": "[label-key='workflow_publishButton'] .umb-button",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Active workflow",
        "content": "<p>When a node is in an active workflow, the Workflow content app displays a collection of informative elements. From this view we can see the original change description, the current workflow state and the progress through the workflow. If the current user can action the pending task, they can do so from this view.</p>",
        "element": ".sub-view-Workflow",
        "elementPreventClick": true
      },
      {
        "title": "Active workflow",
        "content": "<p>Now that we have an active workflow, the dashboard will be more useful. Let's go back to the Content section root.",
        "element": "[data-element='tree-root']",
        "eventElement": "[data-element='tree-root-link']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Active workflow",
        "content": "<p>The dashboard now shows our pending workflow in the My submissions table, with additional UI elements providing a snapshot of workflow progress. Clicking the Detail button opens a modal with the same UI as the Workflow content app (but doesn't play nicely with Backoffice tours). Instead, let's go back the long way, by clicking the node in the content tree.</p>",
        "element": ".umb-box:nth-child(2)",
        "elementPreventClick": true
      },
      {
        "title": "Active workflow",
        "content": "<p>Instead, let's go back the long way, by clicking the node in the content tree.</p>",
        "element": ".umb-tree ul",
        "event": "click",
        "eventElement": ".umb-tree ul .umb-tree-item:first-child a",
        "backdropOpacity": 0.6
      },
      {
        "title": "Active workflow",
        "content": "<p>Once again, jump over the Workflow content app.</p>",
        "element": "[data-element='editor-sub-views']",
        "eventElement": "[data-element='sub-view-workflow']",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "skipStepIfVisible": "workflow-submit",
        "title": "Active workflow",
        "content": "<p>Maybe we didn't want those changes live after all - we can cancel the pending workflow, and the content will be released for editing. Add a short comment, then click the Cancel button.</p>",
        "element": "workflow-action",
        "eventElement": "[label-key='general_cancel'] .umb-button",
        "event": "click",
        "backdropOpacity": 0.6
      },
      {
        "title": "Active workflow",
        "content": "<p>We've now successfully configured a workflow, sent content for approval and actioned changes. There's lots more to Workflow than what this tour covers, explore the documentation to learn more about the configuration and customisation options.</p>",
        "elementPreventClick": true
      }
    ]
  }
]
