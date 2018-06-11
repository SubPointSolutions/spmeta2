module.exports = {

    base: "/spmeta2/",

    markdown: {
        lineNumbers: true,
        config: md => {
            md.use(require('markdown-it-mermaid').default)
        }
    },

    themeConfig: {
        
        // does not work under Docker container, hmm
        //lastUpdated: 'Last Updated',

        repo: 'SubPointSolutions/spmeta2',
        //repoLabel: 'Contribute!',
        docsRepo: 'SubPointSolutions/spmeta2',
        docsDir: 'SPMeta2/SubPointSolutions.Docs/Views/SPMeta2',
        docsBranch: 'dev',
        editLinks: true,
        editLinkText: 'Edit this page',

        nav: [
            { text: 'Home', link: '/' }
        ],

        sidebar: [
            {
                title: 'SPMeta2',
                collapsable: false,
                children: [
                  '/getting-started/',
                  '/getting-started/writing-console-app',
                  '/getting-started/license'
                ]
            },

            {
                title: 'Extensibility',
                collapsable: true,
                children: [
                  //'/extensibility/index',
                  '/extensibility/writing-custom-syntax',
                  '/extensibility/writing-custom-definition',
                  '/extensibility/writing-custom-logging',
                  '/extensibility/regression-testing',
                ]
            },

            {
                title: 'Reference',
                collapsable: true,
                children: [
                  //'/reference/index',
                  '/reference/definitions',
                  '/reference/models',
                  '/reference/provisionservices',
                  '/reference/utils',
                ]
            },
            
        ]
    }
  }